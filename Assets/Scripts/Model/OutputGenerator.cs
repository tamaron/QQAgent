using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;

namespace QQAgent.UI.Model
{

    /// <summary>
    ///  応答生成する基底クラス
    ///  GenerateAsync()を呼び出すとResultに応答文が格納される
    /// </summary>
    abstract public class OutputGenerator
    {
        public string Result { get; protected set; }
        abstract public UniTask<Unit> GenerateAsync(string text = null);
    }

}