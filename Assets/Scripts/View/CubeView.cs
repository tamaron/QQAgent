#pragma warning disable 649
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UniRx;

public class CubeView : MonoBehaviour
{
    Tween _rotateTween;
    Sequence _moveTween;
    [SerializeField] Vector3 rotateTweenEuler;
    [SerializeField] float rotateTweenDuration;
    [SerializeField] float moveTweenDuration;
    [SerializeField] float moveTweenPosX;
    [SerializeField] float tweenPlayMaxSpeed;
    [SerializeField] float tweenPlayMinSpeed;
    [SerializeField] float rotateReachMaxDuration;
    [SerializeField] float rotateReachMinDuration;
    [SerializeField] float backDuration;
    private void Start()
    {
        GameManager.Instance.State
            .Subscribe(s =>
            {
                if (_moveTween != null) _moveTween.Complete();
                _moveTween = DOTween.Sequence();

                switch (s) {
                    case GameState.WaitingInput:
                        _moveTween
                            .Append(
                                transform
                                    .DOLocalMoveX(0, backDuration)
                            )
                            .Join(
                                DOTween.To(
                                    () => _rotateTween.timeScale,
                                    val => _rotateTween.timeScale = val,
                                    1f,
                                    backDuration
                                )
                            );
                        break;


                    case GameState.WaitingOutput:
                        _moveTween
                            .Append(
                                transform
                                    .DOLocalMoveX(moveTweenPosX, moveTweenDuration)
                            )
                            .Join(
                                DOTween.To(
                                    () => 1f,
                                    val => _rotateTween.timeScale = val,
                                    tweenPlayMaxSpeed,
                                    rotateReachMaxDuration
                                )
                            );
                        break;
                    case GameState.Output:
                        if (_moveTween != null) _moveTween.Complete();
                        _moveTween = DOTween.Sequence();
                        _moveTween
                            .Join(
                                DOTween.To(
                                    () => _rotateTween.timeScale,
                                    val => _rotateTween.timeScale = val,
                                    tweenPlayMinSpeed,
                                    rotateReachMinDuration
                                    )
                                );
                        break;

                }
            }).AddTo(this);

        _rotateTween = transform
            .DOLocalRotate(rotateTweenEuler, rotateTweenDuration, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart);
        _rotateTween.timeScale = 1;
    }


}
