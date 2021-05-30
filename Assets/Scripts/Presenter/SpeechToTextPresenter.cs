using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QQAgent.UI.View;
using QQAgent.UI.Model;
using UniRx;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace QQAgent.UI.Presenter
{
    public class SpeechToTextPresenter : MonoBehaviour
    {
        SpeechToTextSender StoTSender;
        SpeechToTextModel StoTModel;

        private void Start()
        {
            StoTSender = SpeechToTextSender.Instance;
            StoTModel = new SpeechToTextModel();
            var tokenSource = new CancellationTokenSource();
            StoTSender.OnListenStart().Subscribe(async _ =>
            {
                SenderControlData.Instance.Listening.Value = true;
                var token = tokenSource.Token;
                string result = await StoTModel.GetSpeechToText(token);
                if (!(result == null))
                {
                    StoTSender.SenderOutputSubject.OnNext(result);
                }
                SenderControlData.Instance.Listening.Value = false;
            }).AddTo(this);
        }
    }

}