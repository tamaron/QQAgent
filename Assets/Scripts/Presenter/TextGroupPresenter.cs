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
    TextGroupView _view;

    private void Awake()
    {
        _model = GetComponent<TextGroupModel>();
        _view = GetComponent<TextGroupView>();
        
        _view.inputField
            .onEndEdit
            .AsObservable()
            .Where(text => text.Length > 0)
            .Subscribe(async text =>
            {
                GameManager.Instance.State.Value = GameState.WaitingOutput;
                _view.resultText.text = await _model.GetResultAsync(text);
                GameManager.Instance.State.Value = GameState.Output;
            }).AddTo(this);
        
    }
}
