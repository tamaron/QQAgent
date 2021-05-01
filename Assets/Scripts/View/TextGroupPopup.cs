#pragma warning disable 649
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRx;
using DG.Tweening;

public class TextGroupPopup : MonoBehaviour
{
    [SerializeField] float fadeDuration;
    [SerializeField] float maxScale;
    [SerializeField] float normalScale;
    [Range(0, 1)] [SerializeField] float ScalePeakTime;

    Sequence _popupSequence;

    private void Start()
    {
        GameStateModel.Instance.State
            .Where(s => s == GameState.WaitingInput)
            .Subscribe(_ => {
                Popup();
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