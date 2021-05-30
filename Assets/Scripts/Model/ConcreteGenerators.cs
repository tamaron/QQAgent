﻿#pragma warning disable CS1998
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using QQAgent.Morpheme;

namespace QQAgent.UI.Model
{
    /// <summary>
    /// NoneMessageを生成する
    /// </summary>
    public class NoneGenerator : OutputGenerator
    {
        public NoneGenerator(AnalyzedInput analyzedInput) : base(analyzedInput) { }
        public async override UniTask<Unit> GenerateAsync()
        {
            Result = MessageTemp.NoneMessage;
            return Unit.Default;
        }
    }

    /// <summary>
    /// 天気予報の応答を生成する
    /// </summary>
    public class WeatherGenerator : OutputGenerator
    {
        public WeatherGenerator(AnalyzedInput analyzedInput) : base(analyzedInput) { }

        const int CELUSIUS = 273;
        public async override UniTask<Unit> GenerateAsync()
        {
            string place = _analyzedInput.Morpheme.Content.FirstOrDefault(e => e.pos1 == "固有名詞" && e.pos2 == "地域")?.surface;
            try
            {
                var querys = new Dictionary<string, string>();
                querys.Add("q", place ?? "仙台");
                string url = "https://community-open-weather-map.p.rapidapi.com/weather?";
                url += await new FormUrlEncodedContent(querys).ReadAsStringAsync();
                HttpClient httpClient = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Add("x-rapidapi-key", Credential.OPEN_WEATHER_MAP_API_KEY);
                request.Headers.Add("x-rapidapi-host", "community-open-weather-map.p.rapidapi.com");
                HttpResponseMessage response = await httpClient.SendAsync(request);
                OpenWeatherMapData data = JsonConvert.DeserializeObject<OpenWeatherMapData>(await response.Content.ReadAsStringAsync());
                if (!response.IsSuccessStatusCode) throw new Exception($"[Cannot get weather in given place : {place}]");
                Result =
                    $"{data.name} の天気" +
                    $"\r\n" +
                    $"天気 : {data.weather[0].main}" +
                    $"\r\n" +
                    $"気温 : { (int)(data.main.temp - CELUSIUS)}度" +
                    $"\r\n" +
                    $"湿度 : { (int)data.main.humidity}%";
            }
            catch (Exception exception)
            {
                Debug.LogError(exception.Message);
                Result = MessageTemp.ErrorMessage;
            }
            return Unit.Default;
        }

        /// <summary>
        /// OpenWeatherMapからデータ受け取りに使う内部クラス
        /// </summary>
        private class OpenWeatherMapData
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

    /// <summary>
    /// だじゃれの評価文を生成する
    /// </summary>
    public class PunGenerator : OutputGenerator
    {
        private string _longestPun;
        public PunGenerator(string pun) => _longestPun = pun;
        public PunGenerator(AnalyzedInput analyzedInput) : base(analyzedInput) { }

        public override async UniTask<Unit> GenerateAsync()
        {
            Result =
                $"{_longestPun} が掛かっているのか...\r\n" +
                $"実に面白い...";
            return Unit.Default;
        }
    }
}