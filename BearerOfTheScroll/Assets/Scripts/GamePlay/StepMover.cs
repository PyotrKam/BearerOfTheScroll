using System;
using System.Collections;
using UnityEngine;

public class StepMover : MonoBehaviour
{
    [Header("Timing")]
    [SerializeField] private float stepDuration = 0.16f;
    [SerializeField] private AnimationCurve ease = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Hop (optional)")]
    [SerializeField] private bool useHop = true;
    [SerializeField] private float hopHeight = 0.06f;

    [Header("SFX (optional)")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] footstepClips;

    private Coroutine moveCo;
    
    public void Play(Vector3 alignedDir, int stepCount, float hexStepLength, Vector3 visualOffset, Action onComplete)
    {
        if (moveCo != null) StopCoroutine(moveCo);
        moveCo = StartCoroutine(PlayRoutine(alignedDir, stepCount, hexStepLength, visualOffset, onComplete));
    }

    private IEnumerator PlayRoutine(Vector3 alignedDir, int stepCount, float hex, Vector3 visOffset, Action onComplete)
    {
        Transform t = transform;
        Vector3 dirXZ = new Vector3(alignedDir.x, 0f, alignedDir.z).normalized;

        float baseY = t.position.y;
        Vector3 startXZ = new Vector3(t.position.x, 0f, t.position.z);

        /*
        if (audioSource != null && footstepClip != null)
            audioSource.PlayOneShot(footstepClip);
        */
        if (audioSource != null && footstepClips != null && footstepClips.Length > 0)
        {
            int idx = UnityEngine.Random.Range(0, footstepClips.Length);
            audioSource.pitch = UnityEngine.Random.Range(0.95f, 1.05f); 
            audioSource.PlayOneShot(footstepClips[idx]);
        }


        for (int i = 1; i <= stepCount; i++)
        {
            Vector3 fromXZ = startXZ + dirXZ * hex * (i - 1);
            Vector3 toXZ = startXZ + dirXZ * hex * i;

            Vector3 from = new Vector3(fromXZ.x, baseY, fromXZ.z) + visOffset;
            Vector3 to = new Vector3(toXZ.x, baseY, toXZ.z) + visOffset;

            float t01 = 0f;
            while (t01 < 1f)
            {
                t01 += Time.deltaTime / Mathf.Max(0.0001f, stepDuration);
                float k = ease != null ? ease.Evaluate(Mathf.Clamp01(t01)) : Mathf.Clamp01(t01);
                            
                Vector3 pos = Vector3.LerpUnclamped(from, to, k);
                                
                if (useHop)
                {
                    float parabola = 4f * k * (1f - k); 
                    pos.y = baseY + parabola * hopHeight;
                }

                t.position = pos;
                yield return null;
            }
                        
            t.position = to;
                        
           
        }

        moveCo = null;
        onComplete?.Invoke();
    }
}
