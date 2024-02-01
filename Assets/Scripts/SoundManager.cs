using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource sfxSource;

    [SerializeField] private AudioSource dialogueSource;

    public static SoundManager instance;

    void Awake()
    {
        instance = this;
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlayDialogue(AudioClip clip)
    {
            dialogueSource.PlayOneShot(clip);
    }
}
