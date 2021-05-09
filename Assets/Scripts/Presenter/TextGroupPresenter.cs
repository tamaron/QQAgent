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

namespace QQAgent.UI.Presenter
{
    public class TextGroupPresenter : MonoBehaviour
    {
        TextGroupModel _model;
        IInputSubmitter _viewInput;
        TextGroupOutput _viewOutput;
        [SerializeField] long processingTimeAdjustMilliseconds;

        private void Awake()
        {
            _model = new TextGroupModel();
            _viewInput = new InputIntegrator();
            _viewOutput = GetComponent<TextGroupOutput>();

            _viewInput.OnInputSubmitted()
                .Subscribe(async text =>
                {
                    GameStateModel.Instance.State.Value = GameState.WaitingOutput;
                    var sw = new Stopwatch();
                    sw.Start();
                    _viewOutput.ResultText = await _model.GetResultAsync(text);
                    sw.Stop();
                    await ProcessingTimeAdjust(sw.ElapsedMilliseconds);
                    GameStateModel.Instance.State.Value = GameState.Output;
                }).AddTo(this);

        }

        private async UniTask<Unit> ProcessingTimeAdjust(long ms)
        {
            long waitTime = processingTimeAdjustMilliseconds - ms;
            if (waitTime <= 0) return Unit.Default;
            await UniTask.Delay((int)waitTime);
            return Unit.Default;
        }
    }
}

