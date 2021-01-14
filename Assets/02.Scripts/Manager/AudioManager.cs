using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Fix
public class AudioManager : MonoBehaviour
{
    [System.Serializable]
    public class Audio
    {
        public string name;
        public AudioClip clip;
    }

    public AudioSource bgmSource;
    public AudioSource sfxSource;
    public Audio[] bgms;
    public Audio[] sfxs;

    private Dictionary<string, AudioClip> bgmDic = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> sfxDic = new Dictionary<string, AudioClip>();

    private void Awake()
    {
        for (int i = 0; i < bgms.Length; i++)
            bgmDic.Add(bgms[i].name, bgms[i].clip);

        for (int i = 0; i < sfxs.Length; i++)
            sfxDic.Add(sfxs[i].name, sfxs[i].clip);
    }
     
    public void PlayBgm(string name)
    {
        if (bgmSource.clip != bgmDic[name])
        {
            bgmSource.clip = bgmDic[name];
            bgmSource.Play();
        }
    }

    public void PlaySfx(string name)
    {
        if (!sfxSource.mute)
        {
            sfxSource.clip = sfxDic[name];
            sfxSource.Play();
        }
    }

    public void Mute(bool isBgm, bool value)
    {
        if (isBgm) bgmSource.mute = value;
        else sfxSource.mute = value;
    }
}
