#pragma warning disable 649
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRx;
using QQAgent.State;

namespace QQAgent.UI.View
{
    public class TextGroupInput : MonoBehaviour
    {
        [SerializeField] private TMP_InputField inputField;

        public IObservable<string> OnInputFieldTextChanged() => inputField.onEndEdit.AsObservable().Where(text => !string.IsNullOrEmpty(text));
        public string InputFieldText
        {
            get { return inputField.text; }
            set { inputField.text = value; }
        }

        private void Start()
        {
            GameStateModel.Instance.State
                .Subscribe(s =>
                {
                // text interactive
                if (s == GameState.WaitingInput)
                    {
                        inputField.interactable = true;
                    }
                    else
                    {
                        inputField.interactable = false;
                    }

                }).AddTo(this);

            // text reset
            GameStateModel.Instance.State
                .Where(s => s == GameState.WaitingInput)
                .Subscribe(s =>
                {
                    InputFieldText = "";
                }).AddTo(this);
        }
    }
}
