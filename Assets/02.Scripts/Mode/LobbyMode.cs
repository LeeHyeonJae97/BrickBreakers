using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyMode : AMode
{
    public AudioManager audioManager;

    public Button startSinglePlay;
    public Button findMatch;
    public Button cancel;

    public override void SetActive(bool value)
    {
        holder.SetActive(value);

        if (value)
        {
            audioManager.PlayBgm("Title");
        }
        else
        {
            ;
        }
    }

    public override void SetMode(ModeManager.SmallMode mode)
    {
        switch (mode)
        {
            case ModeManager.SmallMode.INSERVER:
                status.text = "로비";
                startSinglePlay.interactable = true;
                findMatch.interactable = true;                
                cancel.interactable = false;
                break;
            case ModeManager.SmallMode.FINDINGMATCH:
                status.text = "매칭 상대를 찾는 중";
                startSinglePlay.interactable = false;
                findMatch.interactable = false;
                cancel.interactable = true;
                break;
        }
    }
}
