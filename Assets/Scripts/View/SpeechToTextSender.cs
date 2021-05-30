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
    /// SpeechToText�Ő����������͕���Send����
    /// </summary>
    public class SpeechToTextSender : SingletonMonoBehaviour<SpeechToTextSender>, IInputSender
    {
        [SerializeField] Button button;
        public bool Interactable {
            get => button.interactable;
            set => button.interactable = value;
        }
        // StoTPresenter���w�ǂ���
        public IObservable<Unit> OnListenStart() =>
            button.onClick
            .AsObservable()
            .Where(text => !EventSystem.current.alreadySelecting);
        // StoTPresenter�����ʂ���肵���Ƃ���OnNext����
        public Subject<string> SenderOutputSubject { get; } = new Subject<string>();
        public IObservable<string> OnInputSent() => SenderOutputSubject.Where(txt => !string.IsNullOrEmpty(txt));

        // Interactive��ON =>
        // 1.GameState��WaitingInput
        // 2.Listening�ł͂Ȃ�
        private void Start()
        {
            // Interactive��ON/OFF���؂�ւ��ׂ��^�C�~���O�Ŕ��s�����IObservable
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
