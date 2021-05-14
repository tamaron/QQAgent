using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QQAgent.UI.View;

namespace QQAgent.UI
{
    public class CurrentSender
    {
        static public CurrentSender Instance = new CurrentSender();
        public IInputSender Sender { get; set; } = null;
        public bool Available(IInputSender sender)
        {
            if (Sender == null || ReferenceEquals(sender, Sender)) return true;
            else return false;
        }
    }

}