using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// Sender�Ԃ̐���p�ϐ���ێ�����
/// </summary>
public class SenderControlData
{
    public static SenderControlData Instance { get; } = new SenderControlData();
    public ReactiveProperty<bool> Listening = new ReactiveProperty<bool>(false);
}
