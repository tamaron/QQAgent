using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UniRx;
using QQAgent.State;

namespace QQAgent.UI.View
{
    public class StoTSender : SingletonMonoBehaviour<StoTSender>, IInputSender
    {
        [SerializeField] Button button;
        public bool Interactable {
            get => button.interactable;
            set => button.interactable = value;
        }
        public bool Listening { get; set; } = false;
        // StoTPresenterが購読する
        public IObservable<Unit> OnListenStart() =>
            button.onClick
            .AsObservable()
            .Where(text => !EventSystem.current.alreadySelecting);
        // StoTPresenterが結果を入手したときにOnNextする
        public Subject<string> OutputSubject { get; } = new Subject<string>();
        public IObservable<string> OnInputSent() => OutputSubject.Where(txt => !string.IsNullOrEmpty(txt));

        // InteractiveがON =>
        // 1.GameStateがWaitingInput
        // 2.Listeningではない
        private void Start()
        {
            // TODO:毎フレームアップデートせずいい感じに書き直す
            Observable.EveryUpdate().Subscribe(_ => {
                if (GameStateModel.Instance.State.Value == GameState.WaitingInput
                    && !Listening
                ) Interactable = true;
                else Interactable = false;
            }).AddTo(this);
        }

    }
}
