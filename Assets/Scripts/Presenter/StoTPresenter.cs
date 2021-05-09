using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QQAgent.UI.View;
using QQAgent.UI.Model;
using UniRx;
using Cysharp.Threading.Tasks;

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
            StoTSender.OnButtonClicked().Subscribe(async _ =>
            {
                string result = await StoTModel.GetSpeechToText();
                StoTSender.TextSubject.OnNext(result);
            }).AddTo(this);
        }
    }

}