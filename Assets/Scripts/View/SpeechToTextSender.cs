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
    /// <summary>
    /// SpeechToTextで生成した入力文をSendする
    /// </summary>
    public class SpeechToTextSender : SingletonMonoBehaviour<SpeechToTextSender>, IInputSender
    {
        [SerializeField] Button button;
        public bool Interactable {
            get => button.interactable;
            set => button.interactable = value;
        }
        // StoTPresenterが購読する
        public IObservable<Unit> OnListenStart() =>
            button.onClick
            .AsObservable()
            .Where(text => !EventSystem.current.alreadySelecting);
        // StoTPresenterが結果を入手したときにOnNextする
        public Subject<string> SenderOutputSubject { get; } = new Subject<string>();
        public IObservable<string> OnInputSent() => SenderOutputSubject.Where(txt => !string.IsNullOrEmpty(txt));

        // InteractiveがON =>
        // 1.GameStateがWaitingInput
        // 2.Listeningではない
        private void Start()
        {
            // InteractiveのON/OFFが切り替わるべきタイミングで発行されるIObservable
            var observable = Observable.Create<bool>(observer =>
            {
                var disposable = new CompositeDisposable();
                disposable.Add(
                    GameStateModel.Instance.State
                        .Subscribe(state =>
                        {
                            if (state == GameState.WaitingInput
                                    && !SenderControlData.Instance.Listening.Value
                                ) observer.OnNext(true);
                            else observer.OnNext(false);
                        })
                );
                disposable.Add(
                    SenderControlData.Instance.Listening
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
