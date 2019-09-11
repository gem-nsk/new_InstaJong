using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    public AudioClip[] MusicClips;
    public AudioSource source;

    private int _Currentid = 0;
    private int _currentMusicId
    {
        get
        {
            return _Currentid;
        }
        set
        {
            if(_currentMusicId + value >= MusicClips.Length)
            {
                _currentMusicId = 0;
            }
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
        PlayRepeatMusic();
    }

    public void PlayRepeatMusic()
    {
        StartCoroutine(RepeatMusic());
    }

    public IEnumerator RepeatMusic()
    {
        while(true)
        {
            yield return new WaitForSeconds(PlayMusicClip(_currentMusicId));
            _currentMusicId++;
        }
    }

    float PlayMusicClip(int id)
    {
        source.PlayOneShot(MusicClips[id]);

        Debug.Log("Now playing: " + MusicClips[id].name);
        Debug.Log("Next clip after " + MusicClips[id].length + " seconds");

        return MusicClips[id].length;
    }
}
