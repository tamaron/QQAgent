#pragma warning disable CS1998
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using LitJson;

namespace QQAgent.UI.Model
{
    /// <summary>
    /// NoneMessageを生成する
    /// </summary>
    public class NoneGenerator : OutputGenerator
    {
        public async override UniTask<Unit> GenerateAsync()
        {
            Result = MessageTemp.NoneMessage;
            return Unit.Default;
        }
    }

    /// <summary>
    /// 天気予報を生成する
    /// </summary>

    public class WeatherGenerator : OutputGenerator
    {
        const int CELUSIUS = 273;
        public async override UniTask<Unit> GenerateAsync()
        {
            string url = "https://community-open-weather-map.p.rapidapi.com" +
                "/weather?q=%E4%BB%99%E5%8F%B0&lang=en&units=%22metric%22%20or%20%22imperial%22";
            try
            {
                UnityWebRequest uwr = UnityWebRequest.Get(url);
                // APIKeyはGitHubにあげないように
                uwr.SetRequestHeader("x-rapidapi-key", Credential.OPEN_WEATHER_MAP_API_KEY);
                uwr.SetRequestHeader("x-rapidapi-host", "community-open-weather-map.p.rapidapi.com");

                await uwr.SendWebRequest();
                JsonData jsonData = JsonMapper.ToObject(uwr.downloadHandler.text);
                Result =
                    $"{(string)jsonData["name"]} の天気" +
                    $"\r\n" +
                    $"天気 : {(string)jsonData["weather"][0]["main"]}" +
                    $"\r\n" +
                    $"気温 : { (int)((double)jsonData["main"]["temp"] - CELUSIUS)}度" +
                    $"\r\n" +
                    $"湿度 : { (int)jsonData["main"]["humidity"]}%" +
                    $"\r\n" +
                    $"体感温度 : { (int)((double)jsonData["main"]["feels_like"] - CELUSIUS)}度";

            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                Result = MessageTemp.ErrorMessage;
            }
            return Unit.Default;
        }
    }
}