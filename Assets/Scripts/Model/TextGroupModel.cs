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
    public class TextGroupModel
    {
        public async UniTask<string> GetResultAsync(string text)
        {
            // TODO:カテゴライズに時間がかかるかもしれないのでできれば非同期にしたい
            // コンストラクト時，categorizer.Generatorに適切なResultTextGeneratorの派生クラスが代入される
            MessageTypeCategorizer categorizer = new MessageTypeCategorizer(text);
            ResultTextGenerator generator = categorizer.Generator;
            // 応答文生成
            await generator.GenerateAsync();
            return generator.Result;
        }
    }
}
