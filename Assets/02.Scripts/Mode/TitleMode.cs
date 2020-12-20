using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleMode : AMode
{
    public AudioManager audioManager;

    public InputField nickname;
    public Button connect;
    public Button quit;

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
            case ModeManager.SmallMode.NONE:
                status.text = "";
                nickname.interactable = true;
                connect.interactable = true;
                quit.interactable = true;
                break;
            case ModeManager.SmallMode.CONNECTINGSERVER:
                status.text = "서버 접속 중...";
                nickname.interactable = false;
                connect.interactable = false;
                quit.interactable = false;
                break;
        }
    }
}
