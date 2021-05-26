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
    /// JudgeやGeneratorが応答生成に必要なデータをまとめてある
    /// </summary>
    public class AnalyzedInput
    {
        List<Clause> _morpheme = null;
        public List<Clause> Morpheme
        {
            get => _morpheme;
            set => _morpheme = value;
        }
        public string Text { get; set; }
    }

}