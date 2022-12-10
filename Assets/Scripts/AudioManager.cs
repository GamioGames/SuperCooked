using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioSource CreateSFX(AudioClip clip, float volume = 1, float pitch = 1)
    {
        GameObject sfxInstance = new GameObject();
        AudioSource source = sfxInstance.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;
        source.Play();
        
        Destroy(sfxInstance, clip.length + 0.5f);
        return source;
    }
}
