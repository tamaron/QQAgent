#pragma warning disable CS1998
using System;
using System.Collections;
using System.Collections.Generic;
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

        // 天気=>だじゃれ=>noneの順で判定
        public async UniTask<Unit> CategorizeAsync(string text)
        {
            // AnalyzedInputの作成
            AnalyzedInput analyzedInput = new AnalyzedInput();
            analyzedInput.Text = text;
            IMorphemeAnalyzer analyzer = new MorphemeAnalyzer();
            await analyzer.Analyze(text);
            if (analyzer.Succeeded) analyzedInput.Morpheme = analyzer.Result;

            // 配列の先頭から順に試す
            List<IJudgeable> judgeList = new List<IJudgeable>();
            judgeList.Add(new WeatherJudge());
            judgeList.Add(new PunJudge());
            judgeList.Add(new NoneJudge());
            foreach(var judge in judgeList)
            {
                await judge.JudgeAsync(analyzedInput);
                if (judge.IsMatch)
                {
                    this.Generator = judge.Generator();
                    return Unit.Default;
                }
            }
            throw new Exception("All Generators do not match.");
        }
        public Generator Generator { get; private set; }
    }

    /// <summary>
    /// 入力文がGeneratorに適応可能であるか判定し、Generatorを取り出す
    /// </summary>
    public interface IJudgeable
    {
        public UniTask<Unit> JudgeAsync(AnalyzedInput analyzedInput = null);
        public bool IsMatch { get; }
        public Generator Generator();
    }

    public class NoneJudge : IJudgeable
    {
        public bool IsMatch { get; set; }
        AnalyzedInput _analyzedInput;
        public async UniTask<Unit> JudgeAsync(AnalyzedInput analyzedInput)
        {
            _analyzedInput = analyzedInput;
            IsMatch = true;
            return Unit.Default;
        }
        public Generator Generator() => new NoneGenerator(_analyzedInput);
    }

    public class WeatherJudge : IJudgeable
    {
        public bool IsMatch { get; set; }
        AnalyzedInput _analyzedInput;
        public async UniTask<Unit> JudgeAsync(AnalyzedInput analyzedInput)
        {
            _analyzedInput = analyzedInput;
            IsMatch = Regex.IsMatch(analyzedInput.Text, "天気");
            return Unit.Default;
        }
        public Generator Generator() => new WeatherGenerator(_analyzedInput);
    }

    public class PunJudge : IJudgeable
    {
        public bool IsMatch { get; set; }
        private string _longestPun;

        public async UniTask<Unit> JudgeAsync(AnalyzedInput analyzedInput)
        {
            if (analyzedInput.Morpheme == null)
            {
                IsMatch = false;
                return Unit.Default;
            }
            IsMatch = true;
            MorphemeHandler morphemeHandler = new MorphemeHandler();
            string planeText = await morphemeHandler.GetReadingAsync(analyzedInput.Morpheme);
            PunSearcher punSearcher = new PunSearcher();
            _longestPun = await punSearcher.GetLongestPun(planeText);
            if(_longestPun.Length < 4)
            {
                IsMatch = false;
                return Unit.Default;
            }
            //Debug.Log(analyzedInput.Text);
            //Debug.Log(planeText);
            IsMatch = true;
            return Unit.Default;
        }
        public Generator Generator() => new PunGenerator(_longestPun);
    }
}