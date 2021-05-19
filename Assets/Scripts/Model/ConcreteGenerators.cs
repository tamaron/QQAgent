#pragma warning disable CS1998
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;

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
                Root data = JsonConvert.DeserializeObject<Root>(uwr.downloadHandler.text);
                Result =
                    $"{data.name} の天気" +
                    $"\r\n" +
                    $"天気 : {data.weather[0].main}" +
                    $"\r\n" +
                    $"気温 : { (int)(data.main.temp - CELUSIUS)}度" +
                    $"\r\n" +
                    $"湿度 : { (int)data.main.humidity}%" +
                    $"\r\n" +
                    $"体感温度 : { (int)(data.main.feelslike - CELUSIUS)}度";
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                Result = MessageTemp.ErrorMessage;
            }
            return Unit.Default;
        }

        /// <summary>
        /// OpenWeatherMapからデータ受け取りに使う内部クラス
        /// </summary>
        private class Root
        {
            public class Coord
            {
                public double lon { get; set; }
                public double lat { get; set; }
            }

            public class Weather
            {
                public int id { get; set; }
                public string main { get; set; }
                public string description { get; set; }
                public string icon { get; set; }
            }

            public class Main
            {
                public double temp { get; set; }
                public double feelslike { get; set; }
                public double tempmin { get; set; }
                public double temp_max { get; set; }
                public int pressure { get; set; }
                public int humidity { get; set; }
            }

            public class Wind
            {
                public double speed { get; set; }
                public int deg { get; set; }
            }

            public class Clouds
            {
                public int all { get; set; }
            }

            public class Sys
            {
                public int type { get; set; }
                public int id { get; set; }
                public string country { get; set; }
                public int sunrise { get; set; }
                public int sunset { get; set; }
            }

            public Coord coord { get; set; }
            public List<Weather> weather { get; set; }
            public string @base { get; set; }
            public Main main { get; set; }
            public int visibility { get; set; }
            public Wind wind { get; set; }
            public Clouds clouds { get; set; }
            public int dt { get; set; }
            public Sys sys { get; set; }
            public int timezone { get; set; }
            public int id { get; set; }
            public string name { get; set; }
            public int cod { get; set; }
        }

    }
}