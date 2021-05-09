using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using QQAgent.State;

namespace QQAgent.UI.View
{
    public class StoTSender : SingletonMonoBehaviour<StoTSender>, IInputSender
    {
        [SerializeField] Button button;
        public IObservable<Unit> OnButtonClicked() => button.onClick.AsObservable();
        // Presenterが結果を入手したときにOnNextする
        public Subject<string> TextSubject { get; } = new Subject<string>();
        public IObservable<string> OnInputSent() => TextSubject;
        // ButtonStateに応じて購読する処理を管理する
        private IDisposable disposable = Disposable.Empty;

        private void Start()
        {
            OnButtonClicked().Subscribe(_ => button.interactable = false);
            GameStateModel.Instance.State
                .Where(s => s == GameState.WaitingInput)
                .Subscribe(_ => button.interactable = true);
        }

    }
}
