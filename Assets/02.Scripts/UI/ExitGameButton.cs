using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGameButton : MonoBehaviour
{
    public ModeManager modeManager;

    public void ExitGame()
    {
        modeManager.SetMode(ModeManager.BigMode.GAME, ModeManager.SmallMode.WIN);
        modeManager.SetMode(ModeManager.BigMode.LOBBY, ModeManager.SmallMode.INSERVER);
    }
}
