using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// Sender�Ԃ̐���p�ϐ���ێ�����
/// </summary>
public class SenderControl
{
    public static SenderControl Instance { get; } = new SenderControl();
    public ReactiveProperty<bool> Listening = new ReactiveProperty<bool>(false);
}
