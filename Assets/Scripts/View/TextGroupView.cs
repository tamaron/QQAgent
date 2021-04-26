#pragma warning disable 649
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UniRx;
using TMPro;

public class TextGroupView : MonoBehaviour
{
    [SerializeField] float fadeDuration;
    [Range(0, 1)] [SerializeField] float ScalePeakTime;
    [SerializeField] float normalScale;
    [SerializeField] float maxScale;
    [SerializeField] CanvasGroup resultTextCanvasGroup;
    [SerializeField] float resultTextFadeTime;
    public TextMeshProUGUI resultText;
    public TMP_InputField inputField;

    Sequence _popupSequence;
    Tween resultFadeTween;

    private void Start()
    {
        GameManager.Instance.State
            .Subscribe(s =>
            {
                // text interactive
                if(s == GameState.WaitingInput)
                {
                    inputField.interactable = true;
                }
                else
                {
                    inputField.interactable = false;
                }

                // text reset
                if(s == GameState.WaitingInput)
                {
                    inputField.text = "";
                }

                // answer enable
                if(s == GameState.Output)
                {
                    resultFadeTween.Complete();
                    resultFadeTween =
                        resultTextCanvasGroup.DOFade(1, resultTextFadeTime);
                }

                if (s == GameState.WaitingInput)
                {
                    resultFadeTween.Complete();
                    resultFadeTween =
                        resultTextCanvasGroup.DOFade(0, resultTextFadeTime);
                }

            }).AddTo(this);
    }

    public void Popup()
    {
        _popupSequence = DOTween.Sequence();

        var canvasGroup = GetComponent<CanvasGroup>();
        var rectTransform = GetComponent<RectTransform>();
        canvasGroup
            .DOFade(1, fadeDuration)
            .From(0);

        _popupSequence
            .Append(
                rectTransform
                .DOScale(
                    maxScale, fadeDuration * ScalePeakTime
                    )
            )
            .Append(
                rectTransform
                .DOScale(
                    normalScale, fadeDuration * (1 - ScalePeakTime)
                    )
            );
    }
}
