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
        public ReactiveProperty<bool> Listening { get; set; } = new ReactiveProperty<bool>(false);
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
            var observable = Observable.Create<bool>(observer =>
            {
                var disposable = new CompositeDisposable();
                disposable.Add(
                    GameStateModel.Instance.State
                        .Subscribe(state =>
                        {
                            if (state == GameState.WaitingInput
                                    && !Listening.Value
                                ) observer.OnNext(true);
                            else observer.OnNext(false);
                        })
                );
                disposable.Add(
                    Listening
                        .Subscribe(b =>
                        {
                            if (GameStateModel.Instance.State.Value == GameState.WaitingInput && !b
                                ) observer.OnNext(true);
                            else observer.OnNext(false);
                        })
                );
                return disposable;
            });

            observable.Subscribe(b => Interactable = b).AddTo(this);

            //Observable.EveryUpdate().Subscribe(_ => {
            //    if (GameStateModel.Instance.State.Value == GameState.WaitingInput
            //        && !Listening.Value
            //    ) Interactable = true;
            //    else Interactable = false;
            //}).AddTo(this);
        }

    }
}
