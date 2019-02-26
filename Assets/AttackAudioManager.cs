using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAudioManager : MonoBehaviour {
    public AudioClip[] audioClip = new AudioClip[5];
    private AudioSource audioSource;
    public int attackStatus; // 숫자를 받아와서 해당 사운드를 출력할 수 있도록 함.
    public bool playAudio;
    private int random;
	// Use this for initialization
	void Start () {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClip[0];
        playAudio = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (attackStatus == 1)
        {
            random = Random.Range(0, 2);
            if (random == 0)
                audioSource.clip = audioClip[0];
            else if (random == 1)
                audioSource.clip = audioClip[1];
        }
        else if (attackStatus == 2)
        {
            random = Random.Range(0, 2);
            if (random == 0)
                audioSource.clip = audioClip[2];
            else if (random == 1)
                audioSource.clip = audioClip[3];
        }

        if (playAudio)
        {
            audioSource.PlayOneShot(audioSource.clip);
            playAudio = false;
        }
    }
}
