#pragma warning disable 649
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;

// GameState変更時のゲーム進行に関するコールバックを購読するクラス
public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [SerializeField] GameObject initGroup;
    [SerializeField] GameObject textGroup;
    [SerializeField] float outputToInputDuration;

    protected override void Awake()
    {
        base.Awake();

        GameStateModel.Instance.State
            .Where(s => s == GameState.Entry)
            .Subscribe(_ =>
            {
                initGroup.SetActive(true);
                textGroup.SetActive(false);
            }).AddTo(this);

        GameStateModel.Instance.State
            .Where(s => s == GameState.WaitingInput)
            .Subscribe(_ =>
            {
                textGroup.SetActive(true);
            }).AddTo(this);

        GameStateModel.Instance.State
            .Where(s => s == GameState.Output)
            .Subscribe(_ =>
            {
                DOVirtual.DelayedCall(outputToInputDuration, () => GameStateModel.Instance.State.Value = GameState.WaitingInput);
            }).AddTo(this);
    }
}
