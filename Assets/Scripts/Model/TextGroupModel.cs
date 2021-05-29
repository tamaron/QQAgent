#pragma warning disable 649
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using DG.Tweening;
using UniRx;
using Cysharp.Threading.Tasks;

namespace QQAgent.UI.Model
{
    /// <summary>
    /// 入力文を受け取り応答生成する
    /// </summary>
    public class TextGroupModel
    {
        public async UniTask<string> GetOutputAsync(string text)
        {
            InputCategorizer categorizer = new InputCategorizer();
            await categorizer.CategorizeAsync(text);
            Generator generator = categorizer.Generator;
            // 応答文生成
            await generator.GenerateAsync();
            return generator.Result;
        }
    }
}
