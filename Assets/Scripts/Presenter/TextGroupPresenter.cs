using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System.Threading.Tasks;
using TMPro;

public class TextGroupPresenter : MonoBehaviour
{
    TextGroupModel model;
    TextGroupView view;

    private void Awake()
    {
        model = GetComponent<TextGroupModel>();
        view = GetComponent<TextGroupView>();
        Initiialize();
    }

    private void Initiialize()
    {
        view.inputField
            .onEndEdit
            .AsObservable()
            .Where(text => text.Length > 0)
            .Subscribe(async text =>
            {
                GameManager.Instance.State.Value = GameState.WaitingOutput;
                Task<string> task = Task.Run(() => model.GetResult(text));
                view.resultText.text = await task;
                GameManager.Instance.State.Value = GameState.Output;
            }).AddTo(this);
    }
}
