using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryAudioManager : MonoBehaviour
{
    private AudioSource audioSource;
    private bool playAudio;
    // Use this for initialization
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playAudio = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (playAudio)
        {
            audioSource.PlayOneShot(audioSource.clip);
            playAudio = false;
        }
    }
}
