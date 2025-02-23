using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static SoundManager Instance;

    [SerializeField] private AudioSource EffectSource, RunSource,MuicSource;

    [SerializeField] private AudioClip damageSE;
    [SerializeField] private AudioClip lvlCompletedSE;
    [SerializeField] private AudioClip buttonSE;

    public AudioClip DamageSE { get => damageSE; set => damageSE = value; }
    public AudioClip LvlCompletedSE { get => lvlCompletedSE; set => lvlCompletedSE = value; }
    public AudioClip ButtonSE { get => buttonSE; set => buttonSE = value; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        MuicSource.Play();
    }

    public void RunSound()
    {
        if (!RunSource.isPlaying) // Prevents overlapping sounds
        {
            RunSource.loop = true;
            RunSource.Play();
        }
    }
    public void StopRunSound()
    {
        if (RunSource.isPlaying)
        {
            RunSource.loop = false;
            RunSource.Stop();
        }
    }
    public void PlayEffectSound(AudioClip clipToPlay)
    {
        //if (EffectSource.isPlaying) return;

        EffectSource.PlayOneShot(clipToPlay);
    }


}