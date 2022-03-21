using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
    public AudioClip bgmClip;

    private void Start()
    {
        AudioManager.Instance.BGM.clip = bgmClip;
        if (AudioManager.Instance.BGM.isPlaying == false) {
            AudioManager.Instance.BGM.Play();
        }
    }
}
