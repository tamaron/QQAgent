#pragma warning disable 649
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;


namespace QQAgent.State
{
    /// <summary>
    /// GameStateの情報を保持するクラス
    /// </summary>
    public class GameStateModel
    {
        public static GameStateModel Instance { get; } = new GameStateModel();
        public ReactiveProperty<GameState> State { get; } = new ReactiveProperty<GameState>(GameState.Entry);
    }

    public enum GameState
    {
        Entry,
        WaitingInput,
        WaitingOutput,
        Output,
    }
}
