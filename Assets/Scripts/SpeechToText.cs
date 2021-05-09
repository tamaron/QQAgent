﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Networking;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UniRx;
using Newtonsoft.Json;
using LitJson;
using QQAgent.WavUtil;

namespace QQAgent.Utils
{
    /// <summary>
    /// 非同期でTからUへの翻訳機能
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    public interface ITranslate<T, U>
    {
        UniTask<Unit> Translate(T data);
        U Transcript { get; }
    }

    /// <summary>
    /// 録音機能
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRecord<T>
    {
        T Content { get; }
        bool Recordable { get; }
        void RecStart();
        void RecEnd();
    }

    /// <summary>
    /// AudioClipをテキストに変換する
    /// </summary>
    public class SpeechToText : ITranslate<AudioClip, string>
    {
        const string APIKEY = "";
        const string BASEURl = "https://speech.googleapis.com/v1/speech:recognize";
        string _url = BASEURl + "?key=" + APIKEY;
        const int FLEQUENCY = 16000;
        const string ENCODING = "LINEAR16";
        const string LANGUAGCODE = "ja-JP";
        AudioClip _transData;
        public string Transcript { get; private set; }
        public async UniTask<Unit> Translate(AudioClip data)
        {
            _transData = data;
            byte[] bytes = WavUtility.FromAudioClip(_transData);
            string encodedWav = Convert.ToBase64String(bytes);

            // RequestBodyを作成
            SpeechToTextRequest speechToTextRequest = new SpeechToTextRequest();
            speechToTextRequest.audio.content = encodedWav;

            // RequestBodyをシリアライズ
            string speechToTextRequestJson = JsonConvert.SerializeObject(speechToTextRequest);

            // Requestを作成
            UnityWebRequest uwr = new UnityWebRequest();
            uwr.method = UnityWebRequest.kHttpVerbPOST;
            uwr.url = _url;
            uwr.SetRequestHeader("Content-Type", "application/json");
            uwr.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(speechToTextRequestJson));
            uwr.downloadHandler = new DownloadHandlerBuffer();
            try
            {
                await uwr.SendWebRequest();
                var response = JsonMapper.ToObject(uwr.downloadHandler.text);
                Debug.Log(
                    $"transcript : {response["results"][0]["alternatives"][0]["transcript"]}" +
                    $"\r\n" +
                    $"confidence : { response["results"][0]["alternatives"][0]["confidence"]}"
                    );

                Transcript = (string)response["results"][0]["alternatives"][0]["transcript"];
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                Transcript = "";
            }
            return Unit.Default;
        }

        public class Config
        {
            public string encoding { get; set; } = ENCODING;
            public int sampleRateHertz { get; set; } = FLEQUENCY;
            public string languageCode { get; set; } = LANGUAGCODE;
        }

        public class Audio
        {
            public string content { get; set; }
        }

        public class SpeechToTextRequest
        {
            public Config config { get; set; } = new Config();
            public Audio audio { get; set; } = new Audio();
        }

    }
    /// <summary>
    /// AudioClipの録音を行う
    /// </summary>
    public class Recorder : IRecord<AudioClip>
    {
        const int REC_DURATION_SEC = 5;
        const int FLEQUENCY = 16000;
        public bool Recordable { get; private set; }
        bool _contentAvarable;
        string _micName;
        AudioClip _content;

        public Recorder()
        {
            _contentAvarable = false;
            if (Microphone.devices.Length > 0)
            {
                _micName = Microphone.devices[0];
                Recordable = true;
            }
            else
            {
                Debug.Log($"No Microphone are available");
                Recordable = false;
            }
        }
        public AudioClip Content {
            get
            {
                if (!_contentAvarable)
                {
                    Debug.LogError($"[Error] Content is not available");
                    return null;
                }
                return _content;
            }
            set
            {
                _content = value;
            }
        
        }

        public void RecStart()
        {
            if (!Recordable)
            {
                Debug.LogError($"[Error] Can't record");
                return;
            }
            Content = Microphone.Start(_micName, false, REC_DURATION_SEC, FLEQUENCY);
            Debug.Log("[REC START]");
        }

        // _clipAvarable = trueにするためRecStartを行った場合は必ずあとでRecEnd()をよぶこと
        public void RecEnd()
        {
            if (!Recordable)
            {
                Debug.LogError($"[Error] Can't record");
                return;
            }
            Microphone.End(_micName);
            Debug.Log("[REC STOP]");
            _contentAvarable = true;
        }
    }

}