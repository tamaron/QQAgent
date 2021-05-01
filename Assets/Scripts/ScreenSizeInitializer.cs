using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScreenSizeInitializer
{
    static private int _width = 1024, _height = 768;

    [RuntimeInitializeOnLoadMethod]
    static void OnRuntimeMethodLoad()
    {
        Screen.SetResolution(_width, _height, false);
    }
}
