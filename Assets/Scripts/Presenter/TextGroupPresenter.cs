#pragma warning disable 649
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using QQAgent.State;
using QQAgent.UI.Model;
using QQAgent.UI.View;
using UniRx.Triggers;
using System.Threading.Tasks;
using TMPro;
using System.Threading;

namespace QQAgent.UI.Presenter
{
    public class TextGroupPresenter : MonoBehaviour
    {
        TextGroupModel _model;
        IInputSubmitter _viewInput;
        TextGroupOutput _viewOutput;
        SpeechToTextSender _SpeechToTextView;
        SpeechToTextModel _SpeechToTextModel;
        [SerializeField] long processingTimeAdjustMilliseconds;

        private void Awake()
        {
            // メインのMVP
            _model = new TextGroupModel();
            _viewInput = new InputIntegrator();
            _viewOutput = GetComponent<TextGroupOutput>();

            _viewInput.OnInputSubmitted()
                .Subscribe(async text =>
                {
                    GameStateModel.Instance.State.Value = GameState.WaitingOutput;
                    var sw = new Stopwatch();
                    sw.Start();
                    _viewOutput.ResultText = await _model.GetOutputAsync(text);
                    sw.Stop();
                    await AdjustProcessingTime(sw.ElapsedMilliseconds);
                    GameStateModel.Instance.State.Value = GameState.Output;
                }).AddTo(this);

            // StoT部分のMVP
            _SpeechToTextView = SpeechToTextSender.Instance;
            _SpeechToTextModel = new SpeechToTextModel();
            var tokenSource = new CancellationTokenSource();
            _SpeechToTextView.OnListenStart().Subscribe(async _ =>
            {
                _SpeechToTextView.ListeningSubject.OnNext(true);
                var token = tokenSource.Token;
                string result = await _SpeechToTextModel.GetSpeechToText(token);
                if (!(result == null))
                {
                    _SpeechToTextView.SenderOutputSubject.OnNext(result);
                }
                _SpeechToTextView.ListeningSubject.OnNext(false);
            }).AddTo(this);
        }


        private async UniTask<Unit> AdjustProcessingTime(long ms)
        {
            long waitTime = processingTimeAdjustMilliseconds - ms;
            if (waitTime <= 0) return Unit.Default;
            await UniTask.Delay((int)waitTime);
            return Unit.Default;
        }
    }
}

