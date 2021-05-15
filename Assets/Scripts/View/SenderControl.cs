using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// Sender間の制御用変数を保持する
/// </summary>
public class SenderControl
{
    public static SenderControl Instance { get; } = new SenderControl();
    public ReactiveProperty<bool> Listening = new ReactiveProperty<bool>(false);
}
