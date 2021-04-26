using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;

public class TextGroupModel : MonoBehaviour
{
    [SerializeField] string errorMessage; 
    public string GetResult(string text)
    {
        Thread.Sleep(5000);
        return errorMessage;
    }    
}
