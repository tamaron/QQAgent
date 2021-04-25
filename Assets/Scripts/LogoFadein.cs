using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UniRx;

public class LogoFadein : MonoBehaviour
{
    [SerializeField] float fadeStartTime;
    [SerializeField] float fadeDuration;
    Sequence _sequence;
    private void Start()
    {
        var canvusGroup = GetComponent<CanvasGroup>();
        _sequence = DOTween.Sequence();
        _sequence.Append(
            canvusGroup
                .DOFade(0, fadeDuration)
                .SetDelay(fadeDuration)
            )
        .OnComplete(() =>
        {
            GameManager.Instance.Setup.OnNext(Unit.Default);
            Debug.Log("Comp");
        });
    }

}
