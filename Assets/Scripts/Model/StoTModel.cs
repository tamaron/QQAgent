using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QQAgent.Utils;
using Cysharp.Threading.Tasks;
using UniRx;

namespace QQAgent.UI.Model
{
    public class StoTModel
    {
        const int RECORD_SEC = 5;
        public async UniTask<string> GetSpeechToText()
        {
            IRecorder<AudioClip> recorder = new Recorder();
            if (!recorder.Recordable)
            {
                Debug.LogError("利用可能なマイクがありません");
                return "";
            }
            recorder.RecStart();
            await UniTask.Delay(RECORD_SEC * 1000);
            recorder.RecEnd();
            ITranslater<AudioClip, string> translater = new SpeechToText();
            await translater.Translate(recorder.Content);
            return translater.TranslatedContent;
        }
    }
}