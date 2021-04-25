using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    public Subject<Unit> Setup = new Subject<Unit>();
    [SerializeField] GameObject initGroup;
    [SerializeField] GameObject textGroup;
    protected override void Awake()
    {
        initGroup.SetActive(true);
        textGroup.SetActive(false);
        textGroup.GetComponent<CanvasGroup>().alpha = 0;
        base.Awake();
        Setup.Subscribe(_ =>
        {
            textGroup.SetActive(true);
        }).AddTo(this);
    }
}
