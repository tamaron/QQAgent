using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// Sender間の制御用変数を保持する
/// </summary>
public class SenderControlData
{
    public static SenderControlData Instance { get; } = new SenderControlData();
    public ReactiveProperty<bool> Listening = new ReactiveProperty<bool>(false);
}
