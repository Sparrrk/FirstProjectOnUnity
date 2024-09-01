using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource audioSource;

    public static SoundManager instance;

    private void Awake()
    {
        instance = this; 
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.pitch = Random.Range(0.9f, 1.1f);

        audioSource.PlayOneShot(clip);
    }
}
