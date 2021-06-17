using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QQAgent.Utils;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;

namespace QQAgent.UI.Model
{
    /// <summary>
    /// 音声入力を行い､，テキスト化する
    /// </summary>
    public class SpeechToTextModel
    {
        const int RECORD_SEC = 3;
        public async UniTask<string> GetSpeechToText(CancellationToken token)
        {
            IRecorder<AudioClip> recorder = new Recorder();
            if (!recorder.Recordable)
            {
                return null;
            }
            recorder.RecStart();
            await UniTask.Delay(RECORD_SEC * 1000, false, PlayerLoopTiming.Update, token);
            recorder.RecEnd();
            ITranslater<AudioClip, string> translater = new SpeechToText();
            await translater.Translate(recorder.Content, token);
            return translater.TranslatedContent;
        }
    }
}