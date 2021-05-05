using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UniRx;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using LitJson;

namespace QQAgent.Sample
{

    public class MicTest : MonoBehaviour
    {
        // APIKeyはあげないように
        string APIKey = "";

        void Start()
        {
            var audio = GetComponent<AudioSource>();
            Debug.Log(Microphone.devices[0]);
            var micName = Microphone.devices[0];
            audio.clip = Microphone.Start(micName, false, 8, 16000);
            string encoded;
            Observable.Timer(TimeSpan.FromSeconds(8)).Subscribe(async _ =>
            {
                Microphone.End(micName);
                string filePath;
                byte[] bytes = WavUtility.FromAudioClip(audio.clip, out filePath, false);
                encoded = Convert.ToBase64String(bytes);
                Debug.Log(encoded);
            
                UnityWebRequest uwr = new UnityWebRequest("https://speech.googleapis.com/v1/speech:recognize" + $"?key={APIKey}", UnityWebRequest.kHttpVerbPOST);
                JsonData jsonData = new JsonData();
                jsonData.config.encoding = "LINEAR16";
                jsonData.config.sampleRateHertz = 16000;
                jsonData.config.languageCode = "ja-JP";
                jsonData.audio.content = encoded;
                string sendJson = JsonConvert.SerializeObject(jsonData);
                uwr.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(sendJson));
                uwr.downloadHandler = new DownloadHandlerBuffer();
                uwr.SetRequestHeader("Content-Type", "application/json");
                try
                {
                    await uwr.SendWebRequest();
                    Debug.Log(uwr.downloadHandler.text);
                    var resultObjext = JsonMapper.ToObject(uwr.downloadHandler.text);
                    Debug.Log(resultObjext["results"][0]["alternatives"][0]["confidence"]);
                    Debug.Log(resultObjext["results"][0]["alternatives"][0]["transcript"]);
                }
                catch (Exception e)
                {
                    Debug.Log(e.Message);
                }
            });
        }
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
        public class Config
        {
            public string encoding { get; set; }
            public int sampleRateHertz { get; set; }
            public string languageCode { get; set; }
        }

        public class Audio
        {
            public string content { get; set; }
        }

        public class JsonData
        {
            public Config config { get; set; } = new Config();
            public Audio audio { get; set; } = new Audio();
        }
    }

}