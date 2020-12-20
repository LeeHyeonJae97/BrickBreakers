using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [System.Serializable]
    public class Audio
    {
        public string name;
        public AudioClip clip;
    }

    public AudioSource bgmSource;
    public AudioSource vfxSource;
    public Audio[] bgms;
    public Audio[] vfxs;

    private Dictionary<string, AudioClip> bgmDic = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> vfxDic = new Dictionary<string, AudioClip>();

    private void Awake()
    {
        for (int i = 0; i < bgms.Length; i++)
            bgmDic.Add(bgms[i].name, bgms[i].clip);

        for (int i = 0; i < vfxs.Length; i++)
            vfxDic.Add(vfxs[i].name, vfxs[i].clip);
    }

    public void PlayBgm(string name)
    {
        if (bgmSource.clip != bgmDic[name])
        {
            bgmSource.clip = bgmDic[name];
            bgmSource.Play();
        }
    }

    public void PlayVfx(string name)
    {
        if (!vfxSource.mute)
        {
            vfxSource.clip = vfxDic[name];
            vfxSource.Play();
        }
    }

    public void Mute(bool isBgm, bool value)
    {
        if (isBgm) bgmSource.mute = value;
        else vfxSource.mute = value;
    }
}
