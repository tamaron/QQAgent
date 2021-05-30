using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;

namespace QQAgent.UI.View
{
    /// <summary>
    /// IntegratorにUserInputをSendする
    /// </summary>
    public interface IInputSender
    {
        IObservable<string> OnInputSent();
    }

    /// <summary>
    /// PresenterにUserInputをSubmitする
    /// </summary>
    public interface IInputSubmitter
    {
        IObservable<string> OnInputSubmitted();
    }

    /// <summary>
    /// submitされた入力を画面に反映する
    /// </summary>
    public interface IInputDisplay
    {
        string DisplayText { get; set; }
    }

    /// <summary>
    /// Senderから入力文を受け取り，その内容をPresenterにSubmitする
    /// </summary>
    public class InputIntegrator : IInputSubmitter
    {
        Subject<string> _submitSubject = new Subject<string>();
        public IObservable<string> OnInputSubmitted() => _submitSubject;
        IInputDisplay _display;
        /// <summary>
        /// Senderの管理
        /// </summary>
        List<IInputSender> _senders = new List<IInputSender>();
        public InputIntegrator()
        {
            // 入力した文字を表示する
            _display = InputFieldSender.Instance;

            _senders.Add(InputFieldSender.Instance);
            _senders.Add(SpeechToTextSender.Instance);
            // 各々のSenderを購読する
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
