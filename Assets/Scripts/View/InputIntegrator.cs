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

    public interface IInputDisplay
    {
        string DisPlayText { get; set; }
    }

    /// <summary>
    /// 最終的にPresenterにUserInputをSubmitする
    /// </summary>
    public class InputIntegrator : IInputSubmitter
    {
        Subject<string> _submitSubject = new Subject<string>();
        public IObservable<string> OnInputSubmitted() => _submitSubject;
        IInputDisplay _display;
        List<IInputSender> _senders = new List<IInputSender>();
        public InputIntegrator()
        {
            // 入力した文字を表示する
            _display = InputFieldSender.Instance;
            _senders.Add(InputFieldSender.Instance);

            foreach(var sender in _senders)
            {
                sender.OnInputSent()
                    .Where(txt => !string.IsNullOrEmpty(txt))
                    .Subscribe(txt =>
                    {
                        _submitSubject.OnNext(txt);
                        _display.DisPlayText = txt;
                    });
            }
        }
    }

}
