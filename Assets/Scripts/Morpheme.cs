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
        public UniTask<Morpheme> Analyze(string text);
    }

    /// <summary>
    /// ï∂êﬂÇï\Ç∑
    /// </summary>
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
    /// <summary>
    /// ï∂(ï∂êﬂÇÃÇ†Ç¬Ç‹ÇË)Çï\Ç∑
    /// </summary>
    public class Morpheme {
        public List<Clause> Content { get; set; } = new List<Clause>();
    }


    public class MorphemeAnalyzer : IMorphemeAnalyzer
    {
        HttpClient httpClient = new HttpClient();
        public async UniTask<Morpheme> Analyze(string text)
        {
            var result = new Morpheme(); 
            Dictionary<string, string> parameters = new Dictionary<string, string>()
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
                result.Content = JsonConvert.DeserializeObject<List<Clause>>(jsonData);
            }
            catch(Exception exception)
            {
                Debug.LogError(exception.Message);
                result = null;
            }
            return result;
        }
    }

    public class MorphemeHandler
    {
        public async UniTask<string> GetReadingAsync(Morpheme morpheme) =>
            await UniTask.Run(() => 
                morpheme
                .Content
                .ConvertAll(e => e.reading)
                .Aggregate(new StringBuilder(), (result, next) => result.Append(next))
                .ToString()
            );
    }

}