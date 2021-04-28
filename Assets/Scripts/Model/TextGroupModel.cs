#pragma warning disable 649
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;
using DG.Tweening;
using System.Threading.Tasks;
using UniRx;
using Cysharp.Threading.Tasks;

public class TextGroupModel : MonoBehaviour
{
    [SerializeField] string errorMessage;

    public async UniTask<string> GetResultAsync(string text)
    {
        string url = text;
        string result;
        UnityWebRequest uwr = UnityWebRequest.Get(url);
        try
        {
            await uwr.SendWebRequest();
            result = uwr.downloadHandler.text;
        }
        catch
        {
            result = errorMessage;
        }
        return result;
    }

}
