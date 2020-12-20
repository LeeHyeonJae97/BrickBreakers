using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSinglePlayButton : MonoBehaviour
{
    public ModeManager modeManager;

    public void StartSinglePlay()
    {
        GameManager.isSinglePlay = true;
        modeManager.SetMode(ModeManager.BigMode.GAME, ModeManager.SmallMode.READYFORGAME);
    }
}
