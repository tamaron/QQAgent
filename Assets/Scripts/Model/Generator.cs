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
    abstract public class Generator
    {
        // 応答生成に必要なデータ
        protected AnalyzedInput _analyzedInput;
        public Generator() { }
        public Generator(AnalyzedInput analyzedInput)
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
        List<Clause> _morpheme = null;
        public List<Clause> Morpheme
        {
            get => _morpheme;
            set => _morpheme = value;
        }
        public string Text { get; set; }
    }

}