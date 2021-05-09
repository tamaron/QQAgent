using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using Cysharp.Threading.Tasks;

namespace QQAgent.UI.Model
{

    /// <summary>
    /// 入力された文から適切なGeneratorを返す
    /// </summary>
    public class MessageTypeCategorizer
    {  
        // デフォルトコンストラクタの禁止
        MessageTypeCategorizer() { }

        // Generatorに渡すべき情報(天気を取得したい場所など)がある場合はコンストラクタの引数で渡す
        public MessageTypeCategorizer(string text)
        {
            if (Regex.IsMatch(text, "天気")) Generator = new WeatherGenerator();
            else Generator = new NoneGenerator();
        }
        public OutputGenerator Generator { get; private set; }
    }

}