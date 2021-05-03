using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;

namespace QQAgent.TextGroup
{

    /// <summary>
    ///  応答生成する基底クラス
    ///  Generate()を呼び出すとResultに応答文が格納される
    /// </summary>
    abstract public class ResultTextGenerator
    {
        public string Result { get; protected set; }
        abstract public UniTask<Unit> Generate();
    }

}