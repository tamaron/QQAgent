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
            await analyzer.Analyze("’|’N‚©‚ª—§‚Ä‚©‚¯‚½B’|—§‚ÄŠ|‚¯‚½‚©‚Á‚½‚©‚ç—§‚ÄŠ|‚¯‚½B");
            MorphemeHandler morphemeHandler = new MorphemeHandler();
            if (analyzer.Succeeded)
            {
                Debug.Log(await morphemeHandler.GetReadingAsync(analyzer.Result));
            }
        }
    }
}