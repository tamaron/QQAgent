using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;

namespace QQAgent.UI.View
{
    /// <summary>
    /// Integrator‚ÉUserInput‚ğSend‚·‚é
    /// </summary>
    public interface IInputSender
    {
        IObservable<string> OnInputSent();
    }

    /// <summary>
    /// Presenter‚ÉUserInput‚ğSubmit‚·‚é
    /// </summary>
    public interface IInputSubmitter
    {
        IObservable<string> OnInputSubmitted();
    }

    /// <summary>
    /// submit‚³‚ê‚½“ü—Í‚ğ“ü—Í‰æ–Ê‚É”½‰f‚·‚é
    /// </summary>
    public interface IInputDisplay
    {
        string DisplayText { get; set; }
    }

    /// <summary>
    /// ÅI“I‚ÉPresenter‚ÉUserInput‚ğSubmit‚·‚é
    /// </summary>
    public class InputIntegrator : IInputSubmitter
    {
        Subject<string> _submitSubject = new Subject<string>();
        public IObservable<string> OnInputSubmitted() => _submitSubject;
        IInputDisplay _display;
        List<IInputSender> _senders = new List<IInputSender>();
        public InputIntegrator()
        {
            // “ü—Í‚µ‚½•¶š‚ğ•\¦‚·‚é
            _display = InputFieldSender.Instance;

            _senders.Add(InputFieldSender.Instance);
            _senders.Add(StoTSender.Instance);

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
