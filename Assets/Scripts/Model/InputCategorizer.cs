#pragma warning disable CS1998
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using UniRx;
using Cysharp.Threading.Tasks;

namespace QQAgent.UI.Model
{

    /// <summary>
    /// 入力された文から適切なGeneratorを返す
    /// </summary>
    public class InputCategorizer
    {  

        // 天気=>だじゃれ=>noneの順で判定
        // TODO:判定の優先度とか順番をもっとシステマチックにする
        // Generatorに渡すべき情報(天気を取得したい場所など)がある場合はGeneratorのコンストラクタの引数で渡す
        public async UniTask<Unit> CategorizeAsync(string text)
        {
            // 配列の先頭から順に試す
            List<IJudgeable> judgeList = new List<IJudgeable>();
            judgeList.Add(new WeatherJudge());
            judgeList.Add(new PunJudge());
            judgeList.Add(new NoneJudge());
            foreach(var judge in judgeList)
            {
                await judge.JudgeAsync(text);
                if (judge.IsMatch)
                {
                    this.Generator = judge.Generator();
                    return Unit.Default;
                }
            }
            throw new Exception("All Generators do not match.");
        }
        public OutputGenerator Generator { get; private set; }
    }

    /// <summary>
    /// 入力文がGeneratorに適応可能であるか判定し，可能ならGeneratorを取り出せるようにする
    /// </summary>
    // TODO:GeneratorとJudgeを何かしらで結びつける
    public interface IJudgeable
    {
        public UniTask<Unit> JudgeAsync(string text = null);
        public bool IsMatch { get; }
        public OutputGenerator Generator();
    }

    public class NoneJudge : IJudgeable
    {
        public bool IsMatch { get; set; }

        public async UniTask<Unit> JudgeAsync(string text)
        {
            IsMatch = true;
            return Unit.Default;
        }
        public OutputGenerator Generator() => new NoneGenerator();
    }

    public class WeatherJudge : IJudgeable
    {
        public bool IsMatch { get; set; }

        // TODO 天気に関するワードや地名取得をMecabで行う
        public async UniTask<Unit> JudgeAsync(string text)
        {
            IsMatch = Regex.IsMatch(text, "天気");
            return Unit.Default;
        }
        public OutputGenerator Generator() => new WeatherGenerator();
    }

    public class PunJudge : IJudgeable
    {
        public bool IsMatch { get; set; }

        public async UniTask<Unit> JudgeAsync(string text)
        {

            IsMatch = false;
            return Unit.Default;
        }
        public OutputGenerator Generator() => new PunGenerator();
    }
}