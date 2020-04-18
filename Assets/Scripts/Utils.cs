using UnityEngine;

public static class Utils {
    public static void print(params object[] args) {
        Debug.Log(string.Join(" ", args));
    }
}