using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// SenderŠÔ‚Ì§Œä—p•Ï”‚ğ•Û‚·‚é
/// </summary>
public class SenderControl
{
    public static SenderControl Instance { get; } = new SenderControl();
    public ReactiveProperty<bool> Listening = new ReactiveProperty<bool>(false);
}
