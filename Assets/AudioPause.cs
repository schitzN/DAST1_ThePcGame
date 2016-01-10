using UnityEngine;
using System.Collections;

public class AudioPause : MonoBehaviour {
    AudioSource audio;
	// Use this for initialization
	void Start () {
        this.audio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        if (GameManager.paused && audio.isPlaying)
            audio.Pause();
        else if (!GameManager.paused && !audio.isPlaying)
            audio.Play();
	}
}
