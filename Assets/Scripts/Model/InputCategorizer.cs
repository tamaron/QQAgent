#pragma warning disable CS1998
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text.RegularExpressions;
using UniRx;
using Cysharp.Threading.Tasks;
using QQAgent.Morpheme;
using QQAgent.Pun;

namespace QQAgent.UI.Model
{

    /// <summary>
    /// 入力された文から適切なGeneratorを返す
    /// </summary>
    public class InputCategorizer
    {     
        List<IJudgeable> _judgeList = new List<IJudgeable>{new WeatherJudge(), new PunJudge(), new NoneJudge()};
        IMorphemeAnalyzer _analyzer = new MorphemeAnalyzer();
        // 天気=>だじゃれ=>noneの順で判定
        public async UniTask<OutputGenerator> GetSuitableGeneratorAsync(string text)
        {
            // AnalyzedInputの作成
            AnalyzedInput analyzedInput = new AnalyzedInput();
            analyzedInput.Text = text;
            var morpheme = await _analyzer.Analyze(text);
            if (morpheme != null) analyzedInput.Morpheme = morpheme;

            // 条件を満たす先頭のGeneratorを返す
            return (await UniTask.WhenAll(_judgeList.Select(j => j.JudgeAsync(analyzedInput))))
                .Zip(_judgeList, (result, judge) => (result, judge))
                .FirstOrDefault(pair => pair.result).judge.Generator();
        }
    }

    /// <summary>
    /// 入力文がGeneratorに適応可能であるか判定し、Generatorを取り出す
    /// </summary>
    public interface IJudgeable
    {
        public UniTask<bool> JudgeAsync(AnalyzedInput analyzedInput = null);
        public OutputGenerator Generator();
    }

    public class NoneJudge : IJudgeable
    {
        AnalyzedInput _analyzedInput;
        public async UniTask<bool> JudgeAsync(AnalyzedInput analyzedInput)
        {
            _analyzedInput = analyzedInput;
            return true;
        }
        public OutputGenerator Generator() => new NoneGenerator(_analyzedInput);
    }

    public class WeatherJudge : IJudgeable
    {
        AnalyzedInput _analyzedInput;
        public async UniTask<bool> JudgeAsync(AnalyzedInput analyzedInput)
        {
            _analyzedInput = analyzedInput;
            return analyzedInput.Morpheme.Content.Any(Clause => Clause.surface == "天気");
        }
        public OutputGenerator Generator() => new WeatherGenerator(_analyzedInput);
    }

    public class PunJudge : IJudgeable
    {
        private string _longestPun;
        public async UniTask<bool> JudgeAsync(AnalyzedInput analyzedInput)
        {
            if (analyzedInput.Morpheme == null)
            {
                return false;
            }
            MorphemeHandler morphemeHandler = new MorphemeHandler();
            string planeText = await morphemeHandler.GetReadingAsync(analyzedInput.Morpheme);
            PunSearcher punSearcher = new PunSearcher();
            _longestPun = await punSearcher.GetLongestPun(planeText);
            return (_longestPun.Length < 4);
        }
        public OutputGenerator Generator() => new PunGenerator(_longestPun);
    }
}