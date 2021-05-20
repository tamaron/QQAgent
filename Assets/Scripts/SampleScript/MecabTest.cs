#pragma warning disable CS1998
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text.RegularExpressions;
using UniRx;
using Cysharp.Threading.Tasks;
using QQAgent.Morpheme;

namespace QQAgent.Sample
{
    public class MecabTest : MonoBehaviour
    {
        // Start is called before the first frame update
        async void Start()
        {
            IMorphemeAnalyzer analyzer = new MorphemeAnalyzer();
            await analyzer.Analyze("竹誰かが立てかけた。竹立て掛けたかったから立て掛けた。");
            MorphemeHandler morphemeHandler = new MorphemeHandler();
            if (analyzer.Successed)
            {
                Debug.Log(await morphemeHandler.GetReadingAsync(analyzer.Result));
            }
        }
    }
}