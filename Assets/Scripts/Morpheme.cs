#pragma warning disable CS1998
using System;
using System.Text;
using System.Net.Http;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text.RegularExpressions;
using UniRx;
using Cysharp.Threading.Tasks;

namespace QQAgent.Morpheme
{

    public interface IMorphemeAnalyzer
    {
        public UniTask<Unit> Analyze(string text);
        public bool Successed { get; }
        public Morpheme Result { get; }
    }

    public interface IMorphemeHandler
    {

    }

    public class Morpheme
    {

    }

    public class MorphemeAnalyzer : IMorphemeAnalyzer
    {
        public Morpheme Result { get; set; }
        public bool Successed{ get; set; }

        public async UniTask<Unit> Analyze(string text)
        {
            // RequestÇçÏê¨
            UnityWebRequest uwr = new UnityWebRequest();
            uwr.method = UnityWebRequest.kHttpVerbGET;
            uwr.url = Credential.MECAB_API_URLBASE + "%E4%BB%8A%E6%97%A5%E3%81%AF%E9%9B%A8%E3%81%A0";
            uwr.SetRequestHeader("Content-Type", "application/json");
            uwr.downloadHandler = new DownloadHandlerBuffer();
            try
            {
                await uwr.SendWebRequest();
                Debug.Log(Regex.Unescape(uwr.downloadHandler.text));
            }
            catch (Exception exception)
            {
                Debug.LogError(exception.Message);
            }

            //HttpClient httpClient = new HttpClient();
            //HttpResponseMessage response = await httpClient.GetAsync(Credential.MECAB_API_URLBASE + "%E4%BB%8A%E6%97%A5%E3%81%AF%E9%9B%A8%E3%81%A0");
            //string ret = Regex.Unescape(await response.Content.ReadAsStringAsync());
            //Debug.Log(ret);
            return Unit.Default;
        }
    }

    public class MorphemeHandler : IMorphemeHandler
    {

    }

}