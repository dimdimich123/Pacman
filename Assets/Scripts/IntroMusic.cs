using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroMusic : MonoBehaviour
{
    private AudioSource _audio;
    void Start()
    {
        _audio = GetComponent<AudioSource>();
        Time.timeScale = 0;
        _audio.Play();
    }

    void Update()
    {
        if (!_audio.isPlaying)
        {
            Time.timeScale = 1;
            gameObject.SetActive(false);
        }
    }
}
