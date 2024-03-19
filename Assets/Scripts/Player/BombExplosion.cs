using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Timers;
using UnityEngine;

public class BombExplosion : MonoBehaviour
{
    [SerializeField] private float startSize = 2f;
    [SerializeField] private float startupDur = 0.5f;
    [SerializeField] private float maxSize = 5f;
    [SerializeField] private float maxDur = 0.75f;
    [SerializeField] private float downsizeDur = 2f;

    private void Start()
    {
        StartCoroutine(IExplode());
    }

    IEnumerator IExplode()
    {
        // Animates Explosion
        transform.localScale = Vector3.one * startSize;
        float timer = 0;
        
        // Animate Startup
        while (timer < startupDur)
        {
            timer += Time.deltaTime;
            transform.localScale = Vector3.one * (startSize + ((maxSize-startSize) * timer / startupDur));
            yield return new WaitForEndOfFrame();
        }

        // Animate Hold
        transform.localScale = Vector3.one * maxSize;
        yield return new WaitForSeconds(maxDur);
        
        // Animate downsize
        timer = 0;
        while (timer < downsizeDur)
        {
            timer += Time.deltaTime;
            transform.localScale = maxSize * (1f - (timer / downsizeDur)) * Vector3.one;
            yield return new WaitForEndOfFrame();
        }
        Destroy(gameObject);
    }
}
