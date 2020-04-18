using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    public float time = 5f;

    void Start()
    {
        StartCoroutine(Utils.Delay(()=> Destroy(gameObject), time));
    }
}
