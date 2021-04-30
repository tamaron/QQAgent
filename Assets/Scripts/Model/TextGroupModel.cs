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
                string url = "https://community-open-weather-map.p.rapidapi.com" +
                    "/weather?q=%E4%BB%99%E5%8F%B0&lang=en&units=%22metric%22%20or%20%22imperial%22";
                try
                {
                    UnityWebRequest uwr = UnityWebRequest.Get(url);
                    // APIKeyはGitHubにあげないように
                    uwr.SetRequestHeader("x-rapidapi-key", "");
                    uwr.SetRequestHeader("x-rapidapi-host", "community-open-weather-map.p.rapidapi.com");


                    await uwr.SendWebRequest();
                    JsonData jsonData = JsonMapper.ToObject(uwr.downloadHandler.text);
                    result = $"{(string)jsonData["name"]} の天気" +
                        $"\r\n" +
                        $"{(string)jsonData["weather"][0]["main"]} : {(string)jsonData["weather"][0]["description"]}" + 
                        $"\r\n" +
                        $"気温 : { (int)((double)jsonData["main"]["temp"] / 10)}度" +
                        $"\r\n" +
                        $"湿度 : { (int)jsonData["main"]["humidity"]}%" +
                        $"\r\n" +
                        $"体感温度 : { (int)((double)jsonData["main"]["feels_like"] / 10)}度";

                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
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
