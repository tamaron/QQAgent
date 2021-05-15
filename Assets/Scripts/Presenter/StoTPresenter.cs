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

    public class StoTPresenter : MonoBehaviour
    {
        StoTSender StoTSender;
        StoTModel StoTModel;

        private void Start()
        {
            StoTSender = StoTSender.Instance;
            StoTModel = new StoTModel();
            var tokenSource = new CancellationTokenSource();
            StoTSender.OnListenStart().Subscribe(async _ =>
            {
                SenderControl.Instance.Listening.Value = true;
                var token = tokenSource.Token;
                string result = await StoTModel.GetSpeechToText(token);
                if (!(result == null))
                {
                    StoTSender.SenderOutputSubject.OnNext(result);
                }
                SenderControl.Instance.Listening.Value = false;
            }).AddTo(this);
        }
    }

}