using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip alienDeath;
    private AudioSource audio;

    void Start()
    {
        print("sound manager start");
        audio = GetComponent<AudioSource>();
    }

    public void playClipAtPosition(AudioClip clipToPlay, Vector3 position)
    {
        GameObject soundObject = new GameObject("SoundObject");
        soundObject.transform.position = position;

        AudioSource audioSource = soundObject.AddComponent<AudioSource>();
        audioSource.clip = clipToPlay;
        audioSource.PlayOneShot(clipToPlay);

        Destroy(soundObject);
        
    }

    public void alienDeathSound()
    {
        audio.PlayOneShot(alienDeath);
    }

}
