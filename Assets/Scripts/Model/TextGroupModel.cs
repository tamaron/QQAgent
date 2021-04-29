#pragma warning disable 649
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using DG.Tweening;
using UniRx;
using Cysharp.Threading.Tasks;
using LitJson;

public class TextGroupModel : MonoBehaviour
{
    [SerializeField] string errorMessage;

    public async UniTask<string> GetResultAsync(string text)
    {
        string result;
        switch (GetMessageType(text))
        {
            case MessageType.Weather:
                string url = "https://weather.tsukumijima.net/api/forecast?city=040010";
                try
                {
                    UnityWebRequest uwr = UnityWebRequest.Get(url);
                    await uwr.SendWebRequest();
                    JsonData jsonData = JsonMapper.ToObject(uwr.downloadHandler.text);
                    result = $"{(string)jsonData["title"]}\r\n{(string)jsonData["forecasts"][0]["telop"]}";
                }
                catch
                {
                    result = errorMessage;
                }
                break;
            default:
                result = errorMessage;
                break;

        }
        return result;
    }

    public MessageType GetMessageType(string text)
    {
        if (Regex.IsMatch(text, "天気")) return MessageType.Weather;
        return MessageType.None;
    }
}

public enum MessageType
{
    None,
    Weather
}
