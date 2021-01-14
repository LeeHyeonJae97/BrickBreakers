using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : MonoBehaviour
{
    public AlertConfirm alertConfirm;

    public void OnClick()
    {
        Time.timeScale = 0;
        alertConfirm.Alert("일시정지", UnPause);
    }

    private void UnPause()
    {
        Time.timeScale = 1;
    }
}
