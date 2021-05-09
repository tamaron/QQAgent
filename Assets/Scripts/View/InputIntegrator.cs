using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;

namespace QQAgent.UI.Presenter
{
    public interface IInputSender
    {
        IObservable<string> OnInputSent { get; }
    }

    public interface IInputSubmitter
    {
        IObservable<string> OnInputSubmitted();
    }

    public interface IInputDisplay
    {
        string DisPlayMessage { get; set; }
    }

    /// <summary>
    /// 最終的にPresenterにUserInputを送るクラス
    /// </summary>
    public class InputIntegrator : IInputSubmitter
    {
        Subject<string> _submitSubject = new Subject<string>();
        public IObservable<string> OnInputSubmitted() => _submitSubject;

        IInputDisplay _display;
        List<IInputSender> _senders = new List<IInputSender>();
        public InputIntegrator()
        {
            foreach(var sender in _senders)
            {
                sender.OnInputSent
                    .Where(txt => !string.IsNullOrEmpty(txt))
                    .Subscribe(txt =>
                    {
                        _submitSubject.OnNext(txt);
                        _display.DisPlayMessage = txt;
                    });
            }
        }
    }

}
