using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class ParticleSceneTimer : MonoBehaviour
{
    [Header("Particle Settings")]
    public ParticleSystem[] systems;         // Drag your particle systems here
    [Tooltip("Time (seconds) when particles should start emitting")]
    public float fadeInTime = 270f;          // Example: 4:30 = 270s
    [Tooltip("Time (seconds) when particles should stop emitting")]
    public float fadeOutTime = 480f;         // Example: 8:00 = 480s

    private float timer = 0f;

    void Start()
    {
        // Stop all systems at start
        foreach (var ps in systems)
        {
            if (ps != null)
            {
                ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        // Fade in / play
        if (timer >= fadeInTime && timer < fadeOutTime)
        {
            foreach (var ps in systems)
            {
                if (ps != null && !ps.isPlaying)
                    ps.Play();
            }
        }
        // Fade out / stop
        else if (timer >= fadeOutTime)
        {
            foreach (var ps in systems)
            {
                if (ps != null && ps.isPlaying)
                    ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }
        }
    }
}