using UnityEngine;
using System.Collections;
using System;

public static class Utils {
    public static void print(params object[] args) {
        Debug.Log(string.Join(" ", args));
    }

    public static IEnumerator Delay(Action action, float time) {
        if (time > 0f)
            yield return new WaitForSeconds(time);
        action();
    }
}