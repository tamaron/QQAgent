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
        // StoTPresenter‚ªw“Ç‚·‚é
        public IObservable<Unit> OnListenStart() =>
            button.onClick
            .AsObservable()
            .Where(text => !EventSystem.current.alreadySelecting);
        // StoTPresenter‚ªŒ‹‰Ê‚ğ“üè‚µ‚½‚Æ‚«‚ÉOnNext‚·‚é
        public Subject<string> OutputSubject { get; } = new Subject<string>();
        public IObservable<string> OnInputSent() => OutputSubject.Where(txt => !string.IsNullOrEmpty(txt));

        // Interactive‚ªON =>
        // 1.GameState‚ªWaitingInput
        // 2.Listening‚Å‚Í‚È‚¢
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
