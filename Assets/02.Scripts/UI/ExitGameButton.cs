using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Fix
public class ExitGameButton : MonoBehaviour
{
    public AlertConfirm alertConfirm;
    public ModeManager modeManager;
        
    public void OnClick()
    {
        alertConfirm.Confirm("나가시겠습니까?", ExitGame);
    }

    private void ExitGame()
    {
        modeManager.SetMode(ModeManager.BigMode.GAME, ModeManager.SmallMode.WIN);
        modeManager.SetMode(ModeManager.BigMode.LOBBY, ModeManager.SmallMode.INSERVER);
    }
}
