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
            MorphemeAnalyzer analyzer = new MorphemeAnalyzer();
            await analyzer.Analyze(null);
        }
    }
}