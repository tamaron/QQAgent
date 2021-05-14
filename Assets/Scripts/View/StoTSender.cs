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
        // StoTPresenter���w�ǂ���
        public IObservable<Unit> OnListenStart() =>
            button.onClick
            .AsObservable()
            .Where(text => !EventSystem.current.alreadySelecting);
        // StoTPresenter�����ʂ���肵���Ƃ���OnNext����
        public Subject<string> OutputSubject { get; } = new Subject<string>();
        public IObservable<string> OnInputSent() => OutputSubject.Where(txt => !string.IsNullOrEmpty(txt));

        // Interactive��ON =>
        // 1.GameState��WaitingInput
        // 2.Listening�ł͂Ȃ�
        private void Start()
        {
            // TODO:���t���[���A�b�v�f�[�g�������������ɏ�������
            Observable.EveryUpdate().Subscribe(_ => {
                if (GameStateModel.Instance.State.Value == GameState.WaitingInput
                    && !Listening
                ) Interactable = true;
                else Interactable = false;
            }).AddTo(this);
        }

    }
}
