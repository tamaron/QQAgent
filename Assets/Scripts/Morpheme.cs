using System;
using System.Text;
using System.Linq;
using System.Net.Http;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text.RegularExpressions;
using UniRx;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;


namespace QQAgent.Morpheme
{

    public interface IMorphemeAnalyzer
    {
        public UniTask<Unit> Analyze(string text);
        public bool Successed { get; }
        public List<Clause> Result { get; }
    }


    public class Clause
    {
        public string surface { get; set; }
        public string pos { get; set; }
        public string pos1 { get; set; }
        public string baseform { get; set; }
        public string reading { get; set; }
        public string pronounciation { get; set; }
        public string pos2 { get; set; }
        public string conjugatedform { get; set; }
        public string inflection { get; set; }
    }
    
    public class MorphemeAnalyzer : IMorphemeAnalyzer
    {
        public List<Clause> Result { get; set; } 
        public bool Successed{ get; set; }

        public async UniTask<Unit> Analyze(string text)
        {
            HttpClient httpClient = new HttpClient();
            var parameters = new Dictionary<string, string>()
            {
                { "sentence", text },
            };
            try
            {
                var encoded = new FormUrlEncodedContent(parameters);
                var parameter = await encoded.ReadAsStringAsync();
                HttpResponseMessage response = await httpClient.GetAsync(
                    $"{Credential.MECAB_API_URLBASE}" +
                    $"?" +
                    $"{parameter}"
                );
                string jsonData = await response.Content.ReadAsStringAsync();
                jsonData = Regex.Unescape(jsonData);
                Result = JsonConvert.DeserializeObject<List<Clause>>(jsonData);
                Successed = true;
            }
            catch(Exception exception)
            {
                Debug.LogError(exception.Message);
                Successed = false;
            }
            return Unit.Default;
        }
    }

    public class MorphemeHandler
    {
        public async UniTask<string> GetReadingAsync(List<Clause> morpheme) =>
            await UniTask.Run(() => 
                morpheme
                .ConvertAll(e => e.reading)
                .Aggregate(new StringBuilder(), (result, next) => result.Append(next))
                .ToString()
            );
    }

}