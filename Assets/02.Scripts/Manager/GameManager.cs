using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Fix
public class GameManager : MonoBehaviour
{
    public static bool isSinglePlay;

    private void Awake()
    {
        ScaleResolution();
    }

    private void OnPreCull()
    {
        GL.Clear(true, true, Color.black);
    }

    private void ScaleResolution()
    {
        Camera camera = Camera.main;
        Rect rect = camera.rect;
        float scaleheight = ((float)Screen.width / Screen.height) / ((float)1080 / 1920); // (가로 / 세로)
        float scalewidth = 1f / scaleheight;
        if (scaleheight < 1)
        {
            rect.height = scaleheight;
            rect.y = (1f - scaleheight) / 2f;
        }
        else
        {
            rect.width = scalewidth;
            rect.x = (1f - scalewidth) / 2f;
        }
        camera.rect = rect;
    }

    public void QuitGame() => Application.Quit();
}
