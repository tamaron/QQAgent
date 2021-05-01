#pragma warning disable 649
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UniRx;

public class LogoView : MonoBehaviour
{
    [SerializeField] float fadeStartTime;
    [SerializeField] float fadeDuration;
    Sequence _sequence;
    private void Start()
    {
        Fadein();
    }

    void Fadein()
    {
        var canvusGroup = GetComponent<CanvasGroup>();
        _sequence = DOTween.Sequence();
        _sequence.Append(
            canvusGroup
                .DOFade(0, fadeDuration)
                .SetDelay(fadeStartTime)
            )
        .OnComplete(() =>
        {
            // Entry => WaitingInput
            GameStateModel.Instance.State.Value = GameState.WaitingInput;
        });
    }

}
