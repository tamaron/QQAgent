using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using UniRx.Triggers;
using System.Threading.Tasks;
using TMPro;

public class TextGroupPresenter : MonoBehaviour
{
    TextGroupModel _model;
    TextGroupInput _viewInput;
    TextGroupOutput _viewOutput;

    private void Awake()
    {
        _model = GetComponent<TextGroupModel>();
        _viewInput = GetComponent<TextGroupInput>();
        _viewOutput = GetComponent<TextGroupOutput>();

        _viewInput.OnInputFieldTextChanged()
            .Where(text => !string.IsNullOrEmpty(text))
            .Subscribe(async text =>
            {
                GameStateModel.Instance.State.Value = GameState.WaitingOutput;
                _viewOutput.ResultText = await _model.GetResultAsync(text);
                GameStateModel.Instance.State.Value = GameState.Output;
            }).AddTo(this);
        
    }
}
