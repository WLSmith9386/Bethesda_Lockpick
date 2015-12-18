using UnityEngine;
using System.Collections;

public class AudioControl : MonoBehaviour {

    public AudioSource speaker;

    void Start()
    {
        if (speaker == null)
        {
            gameObject.AddComponent<AudioSource>();

            speaker = gameObject.GetComponent<AudioSource>();
        }
    }

    public void PlaySound(AudioClip sound)
    {
        speaker.clip = sound;
        speaker.Play();
    }

    public void PlaySoundIfNotPlaying(AudioClip sound)
    {
        if (!speaker.isPlaying)
        {
            speaker.clip = sound;
            speaker.Play();
        }
    }

    public void StopPlayingSound()
    {
        if (speaker.isPlaying)
        {
            speaker.Stop();
        }
    }
}
