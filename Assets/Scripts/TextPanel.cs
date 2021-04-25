using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UniRx;

public class TextPanel : MonoBehaviour
{
    [SerializeField] float fadeDuration;

    private void Start()
    {
        var canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup
            .DOFade(1, fadeDuration)
            .From(0);
    }
}
