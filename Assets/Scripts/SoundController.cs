using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    [SerializeField] private List<AudioClip> newClip; 
    public static SoundController Instance { get; private set;}

    void Awake() => CheckSingleton();
    
    private void CheckSingleton()
    {
        if(Instance != this && Instance != null) Destroy(this);
        else Instance = this;
    }
    public void PlaySound(string clipName, float volume)
    {
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume *= volume;
        audioSource.PlayOneShot((AudioClip) Resources.Load("Sounds/" + clipName, typeof(AudioClip)));
    }
    public void PlayBackGround(int clipNumber)
    {
       GameObject bgMusic = GameObject.Find("BackgroundMusic");
       AudioSource audioSource = bgMusic.GetComponent<AudioSource>();
       audioSource.clip = newClip[clipNumber];
       audioSource.Play();
    }

}
