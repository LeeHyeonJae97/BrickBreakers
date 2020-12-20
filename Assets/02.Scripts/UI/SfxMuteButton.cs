using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SfxMuteButton : MonoBehaviour
{
    public AudioManager audioManager;

    public Image image;
    public Sprite on;
    public Sprite off;

    private bool isMute;

    public void Mute()
    {
        isMute = !isMute;
        image.sprite = isMute ? off : on;
        audioManager.Mute(false, isMute);
    }
}
