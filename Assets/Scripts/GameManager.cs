#pragma warning disable 649
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    static int _width = 1024, _height = 768;
    public ReactiveProperty<GameState> State { get; } = new ReactiveProperty<GameState>(GameState.Entry);
    [RuntimeInitializeOnLoadMethod]
    static void OnRuntimeMethodLoad()
    {
        Screen.SetResolution(_width, _height, false);
    }

    [SerializeField] GameObject initGroup;
    [SerializeField] GameObject textGroup;
    protected override void Awake()
    {
        base.Awake();
        initGroup.SetActive(true);
        textGroup.SetActive(false);
        textGroup.GetComponent<CanvasGroup>().alpha = 0;
        State
            .Where(s => s == GameState.WaitingInput)
            .Subscribe(_ =>
            {
                textGroup.SetActive(true);
                textGroup.GetComponent<TextGroupView>().Popup();
            }).AddTo(this);
        State
            .Where(s => s == GameState.Output)
            .Subscribe(_ =>
            {
                DOVirtual.DelayedCall(3f, () => State.Value = GameState.WaitingInput);
            }).AddTo(this);
    }
}

public enum GameState
{
    Entry,
    WaitingInput,
    WaitingOutput,
    Output,
}
