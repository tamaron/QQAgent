using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using TMPro;

public class TextGroupPresenter : MonoBehaviour
{
    TextGroupModel model;
    TextGroupView view;

    private void Awake()
    {
        model = GetComponent<TextGroupModel>();
        view = GetComponent<TextGroupView>();

        view.inputField
            .onEndEdit
            .AsObservable() 
            .Subscribe(text =>
            {
                Debug.Log(text);
                view.inputField.text = "";
                view.inputField.Select();
            }).AddTo(this);
    }
}
