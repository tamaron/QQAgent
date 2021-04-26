#pragma warning disable 649
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Cube : MonoBehaviour
{
    Tween _tween;
    float _tweenPlaySpeed = 1f;
    [SerializeField] Vector3 tweenEuler;
    [SerializeField] float tweenDuration;
    private void Start()
    {
        _tween = transform
            .DOLocalRotate(tweenEuler, tweenDuration, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart);
        _tween.timeScale = _tweenPlaySpeed;
    }


}
