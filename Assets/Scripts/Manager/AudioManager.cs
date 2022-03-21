using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public enum audioType { background, soundEffect };

[System.Serializable]
public class Sound {

    public string name;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;

    [Range(.1f, 3f)]
    public float pitch;

    [HideInInspector]
    public AudioSource source;

    public bool isLoop;

    public AudioMixerGroup audioGroup;
}

public class AudioManager : Singleton<AudioManager>
{
    public Sound[] sounds;

    public Sound[] effects;

    public AudioSource BGM;

    public AudioMixerGroup bgmMixerGroup;
    public AudioMixerGroup soundEffectsGroup;


    public AudioMixer myAudioMixer;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;

            s.source.loop = s.isLoop;
            s.source.playOnAwake = false;

            s.source.outputAudioMixerGroup = s.audioGroup;
        }

        foreach (Sound e in effects)
        {
            e.source = gameObject.AddComponent<AudioSource>();
            e.source.clip = e.clip;

            e.source.volume = e.volume;
            e.source.pitch = e.pitch;

            e.source.loop = e.isLoop;

            e.source.playOnAwake = false;

            e.source.outputAudioMixerGroup = e.audioGroup;
        }

        BGM.outputAudioMixerGroup = bgmMixerGroup;
    }

    public void PlayMusic(string name) {

        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }

    public void PlaySfx(string name)
    {

        Sound e = Array.Find(effects, efx => efx.name == name);
        e.source.Play();
    }

    public void SetMusicLvl(float musicLevel) {
        myAudioMixer.SetFloat("musicVol", musicLevel);
        //bgmMixerGroup.audioMixer.SetFloat("BGM", musicLevel);
    }

    public void SetSfxLvl(float sfxLevel)
    {
        myAudioMixer.SetFloat("sfxVol", sfxLevel);
        //soundEffectsGroup.audioMixer.SetFloat("SoundEffect", sfxLevel);
    }
}
