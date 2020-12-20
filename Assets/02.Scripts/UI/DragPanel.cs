using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragPanel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public BallManager ballManager;
    public LineRenderer lr;
    public Camera mainCam;

    private bool isPressed;
    private Vector2 first;
    private Vector2 second;
    private Vector2 dir;

    private void OnDisable()
    {
        lr.enabled = false;
    }

    private void Update()
    {
        if (isPressed)
        {
            second = mainCam.ScreenToWorldPoint(Input.mousePosition);
            dir = (second - first).normalized;
            lr.SetPosition(1, first + dir * 2);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        first = ballManager.GatherPos;
        second = mainCam.ScreenToWorldPoint(Input.mousePosition);

        if (second.y > first.y)
        {
            isPressed = true;
            lr.enabled = true;
            lr.SetPosition(0, first);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
        lr.enabled = false;
        ballManager.Shoot(dir);
    }
}
