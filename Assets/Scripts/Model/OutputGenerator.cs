using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using QQAgent.Morpheme;
using System.Threading.Tasks;

namespace QQAgent.UI.Model
{

    /// <summary>
    ///  応答生成する基底クラス
    ///  GenerateAsync()を呼び出すとResultに応答文が格納される
    /// </summary>
    abstract public class OutputGenerator
    {
        // 応答生成に必要なデータ
        protected AnalyzedInput _analyzedInput;
        public OutputGenerator() { }
        public OutputGenerator(AnalyzedInput analyzedInput)
        {
            _analyzedInput = analyzedInput;
        }
        public string Result { get; protected set; }
        abstract public UniTask<Unit> GenerateAsync();
    }

    /// <summary>
    /// 応答生成に必要なデータをまとめたクラス
    /// </summary>
    public class AnalyzedInput
    {
        public Morpheme.Morpheme Morpheme{ get; set; }
        public string Text { get; set; }
    }

}