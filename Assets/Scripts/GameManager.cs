using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    public Subject<Unit> Setup = new Subject<Unit>();
    [SerializeField] GameObject textPanel;
    protected override void Awake()
    {
        base.Awake();
        Setup.Subscribe(_ =>
        {
            textPanel.SetActive(true);
        }).AddTo(this);
    }
}
