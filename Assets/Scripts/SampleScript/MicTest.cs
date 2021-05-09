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
using QQAgent.WavUtil;
using QQAgent.Utils;

namespace QQAgent.Sample
{

    public class MicTest : MonoBehaviour
    {
        private void Start()
        {
            IRecorder<AudioClip> recorder = new Recorder();
            if (!recorder.Recordable) return;
            recorder.RecStart();
            Observable.Timer(TimeSpan.FromSeconds(5)).Subscribe(async _ =>
            {
                recorder.RecEnd();
                ITranslater<AudioClip, string> translater = new SpeechToText();
                await translater.Translate(recorder.Content);
                Debug.Log(translater.TranslatedContent);

            }).AddTo(this);
        }
    }
}