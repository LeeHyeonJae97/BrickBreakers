using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundMuteButton : MonoBehaviour
{
    public AudioManager audioManager;

    public Image image;
    public Sprite on;
    public Sprite off;

    public bool isBgm;
    private bool isMute = true;

    public void Mute()
    {
        isMute = !isMute;
        image.sprite = isMute ? off : on;
        audioManager.Mute(isBgm, isMute);
    }
}
