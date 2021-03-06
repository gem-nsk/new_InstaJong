﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    public AudioClip[] MusicClips;
    public AudioSource source;

    private int _Currentid = 0;
   

    private bool _isPlaying;
    public bool isPlaying {
        get
        {
            return _isPlaying;
        }
        set
        {
            _isPlaying = value;
            //Debug.Log(_isPlaying);
        }
    }

    #region Singleton
    public static Music instance;
    private void Awake()
    {
        GameObject[] Objects = GameObject.FindGameObjectsWithTag("Music");
        if(Objects.Length == 1)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    private void Start()
    {
        int i = PlayerPrefs.GetInt("Music");
        isPlaying = PlayerPrefs.HasKey("Music") ? ConvertIntToBool(i) : true;
        PlayRepeatMusic();
    }
    bool ConvertIntToBool(int i)
    {
        switch (i)
        {
            case 0:
                return false;
            case 1:
                return true;
            default:
                return false;
        }
    }

    public void PlayRepeatMusic()
    {
        StartCoroutine(RepeatMusic());
    }

    public void TurnOn()
    {
        if (isPlaying)
        {
            source.UnPause();
        }
    }

    public void TurnOff()
    {
        if (isPlaying)
        {
            source.Pause();
        }
    }

    public bool SwitchMusic()
    {
        isPlaying = !isPlaying;
        PlayerPrefs.SetInt("Music", isPlaying? 1 : 0);
        switch(isPlaying)
        {
            case true:
                PlayRepeatMusic();
                break;
            case false:
                StopMusic();
                break;
        }
        return isPlaying;
    }

    public IEnumerator RepeatMusic()
    {
        while(isPlaying)
        {
            _Currentid = Random.Range(0, MusicClips.Length);
            yield return new WaitForSecondsRealtime(PlayMusicClip(_Currentid));
        }
    }

    float PlayMusicClip(int id)
    {
        source.Stop();
        source.clip = null;

        source.PlayOneShot(MusicClips[id]);

        //Debug.Log("Now playing: " + MusicClips[id].name);
        //Debug.Log("Next clip after " + MusicClips[id].length + " seconds");

        return MusicClips[id].length;
    }

    public void StopMusic()
    {
        source.Stop();
    }
}
