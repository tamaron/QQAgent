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

namespace QQAgent.TextGroup
{
    public class TextGroupModel
    {
        public async UniTask<string> GetResultAsync(string text)
        {
            // コンストラクト時，categorizer.Generatorに適切なMessageTypeCategorizerの派生クラスが代入される
            MessageTypeCategorizer categorizer = new MessageTypeCategorizer(text);
            ResultTextGenerator generator = categorizer.Generator;
            await generator.Generate();
            return generator.Result;
        }
    }
}
