using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAudioManager : MonoBehaviour {
    private AudioSource audioSource;
    public bool playAudio;
	// Use this for initialization
	void Start () {
        audioSource = GetComponent<AudioSource>();
        playAudio = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (playAudio) {
            audioSource.PlayOneShot(audioSource.clip);
            playAudio = false;
        }
    }
}
