#pragma warning disable 649
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    static int _width = 1024, _height = 768;
    [RuntimeInitializeOnLoadMethod]
    static void OnRuntimeMethodLoad()
    {
        Screen.SetResolution(_width, _height, false);
    }

    public Subject<Unit> Setup = new Subject<Unit>();
    [SerializeField] GameObject initGroup;
    [SerializeField] GameObject textGroup;
    protected override void Awake()
    {
        base.Awake();
        initGroup.SetActive(true);
        textGroup.SetActive(false);
        textGroup.GetComponent<CanvasGroup>().alpha = 0;
        Setup.Subscribe(_ =>
        {
            textGroup.SetActive(true);
        }).AddTo(this);
    }
}
