using System;
using System.Text;
using System.Linq;
using System.Net.Http;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text.RegularExpressions;
using UniRx;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;


namespace QQAgent.Pun
{
    public class PunSearcher
    {
        public async UniTask<string> GetLongestPun(string text)
        {
            int size = text.Length;
            Debug.Log($"{size} {text}");
            int[,] table = new int[size + 1, size + 1];
            int max = 0;
            (int i, int j) locate = (-1, -1);
            await UniTask.Run(() => { 
                for (int j = size - 1; 0 <= j; j--) for (int i = j - 1; 0 <= i; i--)
                {
                    if (text[i] == text[j]) table[i, j] = Math.Min(table[i + 1, j + 1] + 1, j - i);
                    if (max <= table[i, j])
                    {
                        max = table[i, j];
                        locate = (i, j);
                    }
                    Debug.Log($"{i} {j} {table[i, j]}");
                }
            });
            Debug.Log($"{locate.i} {locate.j}");
            return text.Substring(locate.i, max);
        } 
    }
}