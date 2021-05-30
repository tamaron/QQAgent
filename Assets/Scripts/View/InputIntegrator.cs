using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;

namespace QQAgent.UI.View
{
    /// <summary>
    /// Integrator��UserInput��Send����
    /// </summary>
    public interface IInputSender
    {
        IObservable<string> OnInputSent();
    }

    /// <summary>
    /// Presenter��UserInput��Submit����
    /// </summary>
    public interface IInputSubmitter
    {
        IObservable<string> OnInputSubmitted();
    }

    /// <summary>
    /// submit���ꂽ���͂���ʂɔ��f����
    /// </summary>
    public interface IInputDisplay
    {
        string DisplayText { get; set; }
    }

    /// <summary>
    /// Sender������͕����󂯎��C���̓��e��Presenter��Submit����
    /// </summary>
    public class InputIntegrator : IInputSubmitter
    {
        Subject<string> _submitSubject = new Subject<string>();
        public IObservable<string> OnInputSubmitted() => _submitSubject;
        IInputDisplay _display;
        /// <summary>
        /// Sender�̊Ǘ�
        /// </summary>
        List<IInputSender> _senders = new List<IInputSender>();
        public InputIntegrator()
        {
            // ���͂���������\������
            _display = InputFieldSender.Instance;

            _senders.Add(InputFieldSender.Instance);
            _senders.Add(SpeechToTextSender.Instance);
            // �e�X��Sender���w�ǂ���
            foreach (var sender in _senders)
            {
                sender.OnInputSent()
                    .Subscribe(txt =>
                    {
                        _submitSubject.OnNext(txt);
                        _display.DisplayText = txt;
                    });
            }
        }
    }

}
