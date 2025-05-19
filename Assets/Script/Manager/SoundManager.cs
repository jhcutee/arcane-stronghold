using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Elements")]
    [Header("GameSounds")]
    [SerializeField] private AudioClip levelCompletedAudio;
    [SerializeField] private AudioClip gameLoseAudio;
    [SerializeField] private AudioClip gameWinAudio;

    [SerializeField] private AudioSource bgAudioSource;
    private void Awake()
    {
        bgAudioSource.Play();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
