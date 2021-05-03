#pragma warning disable 649
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UniRx;
using TMPro;
using QQAgent.State;


namespace QQAgent.UI.View
{
    public class TextGroupOutput : MonoBehaviour
    {
        [SerializeField] CanvasGroup outputTextCanvasGroup;
        [SerializeField] float outputTextFadeTime;
        [SerializeField] private TextMeshProUGUI outputText;
        Tween outputFadeTween;

        public string ResultText
        {
            get { return outputText.text; }
            set { outputText.text = value; }
        }

        private void Start()
        {
            GameStateModel.Instance.State
                .Subscribe(s =>
                {
                // result text enable
                if (s == GameState.Output)
                    {
                        outputFadeTween.Complete();
                        outputFadeTween =
                            outputTextCanvasGroup.DOFade(1, outputTextFadeTime);
                    }

                    if (s == GameState.WaitingInput)
                    {
                        outputFadeTween.Complete();
                        outputFadeTween =
                            outputTextCanvasGroup.DOFade(0, outputTextFadeTime)
                            .OnComplete(() => ResultText = "");
                    }

                }).AddTo(this);
        }
    }
}