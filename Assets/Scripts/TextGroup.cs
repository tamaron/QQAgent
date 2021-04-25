using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UniRx;

public class TextGroup : MonoBehaviour
{
    [SerializeField] float fadeDuration;
    [Range(0, 1)] [SerializeField] float ScalePeakTime;
    [SerializeField] Vector3 normalScale;
    [SerializeField] Vector3 maxScale;

    Sequence _sequence;

    private void Start()
    {
        _sequence = DOTween.Sequence();
        var canvasGroup = GetComponent<CanvasGroup>();
        var rectTransform = GetComponent<RectTransform>();
        canvasGroup
            .DOFade(1, fadeDuration)
            .From(0);

        _sequence
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
