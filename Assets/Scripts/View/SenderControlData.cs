using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// SenderŠÔ‚Ì§Œä—p•Ï”‚ğ•Û‚·‚é
/// </summary>
public class SenderControlData
{
    public static SenderControlData Instance { get; } = new SenderControlData();
    public ReactiveProperty<bool> Listening = new ReactiveProperty<bool>(false);
}
