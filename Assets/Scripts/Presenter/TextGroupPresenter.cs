using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using UniRx.Triggers;
using System.Threading.Tasks;
using TMPro;

namespace QQAgent.TextGroup
{
    public class TextGroupPresenter : MonoBehaviour
    {
        TextGroupModel _model;
        TextGroupInput _viewInput;
        TextGroupOutput _viewOutput;

        private void Awake()
        {
            _model = new TextGroupModel();
            _viewInput = GetComponent<TextGroupInput>();
            _viewOutput = GetComponent<TextGroupOutput>();

            _viewInput.OnInputFieldTextChanged()
                .Subscribe(async text =>
                {
                    GameStateModel.Instance.State.Value = GameState.WaitingOutput;
                    _viewOutput.ResultText = await _model.GetResultAsync(text);
                    GameStateModel.Instance.State.Value = GameState.Output;
                }).AddTo(this);

        }
    }
}
