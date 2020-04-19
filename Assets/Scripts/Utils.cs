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

    public static (bool, Vector3) TryShootAt(Vector3 shooter, float shooterHeight, float shooterRadius, float shooterRange, Vector3 target, float targetHeight, float targetRadius) {
        var dir = target - shooter;
        if (dir.sqrMagnitude > shooterRange*shooterRange) {
            return (false, target);
        }
        shooter.y += shooterHeight;
        target.y += UnityEngine.Random.Range(0f, targetHeight);
        target += UnityEngine.Random.Range(-targetRadius, targetRadius) * new Vector3(-dir.z, dir.x).normalized;
        dir = shooter - target;
        float dist = dir.magnitude;
        if (dist > shooterRange)
            return (false, target);
        dir /= dist;
        if (Physics.Raycast(target + dir * targetRadius, dir, dist - shooterRadius))
            return (false, target);
        return (true, target);
    }

    public static (float, Vector3) Danger(Vector3 shooter, float shooterHeight, float shooterRadius, float shooterRange, Vector3 target, float targetHeight, float targetRadius, int rays = 20) {
        (bool a, Vector3 hit) = TryShootAt(shooter, shooterHeight, shooterRadius, shooterRange, target, targetHeight, targetRadius);
        int hits = a? 1: 0;
        for (int i = 1; i < rays; i++)
        {
            if (TryShootAt(shooter, shooterHeight, shooterRadius, shooterRange, target, targetHeight, targetRadius).Item1) {
                hits++;
            }
        }
        return ((float)hits / (float)rays, hit);
    }
}