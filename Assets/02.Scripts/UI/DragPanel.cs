using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Fix
public class DragPanel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public BallManager ballManager;
    public LineRenderer lr;
    public Camera mainCam;

    private Vector2 first;
    private Vector2 dir;

    private void OnDisable()
    {
        lr.enabled = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        first = ballManager.GatherPos;
        dir = ((Vector2)mainCam.ScreenToWorldPoint(Input.mousePosition) - first).normalized;

        if (dir.y > 0)
        {
            lr.enabled = true;
            lr.SetPosition(0, first);
            lr.SetPosition(1, first + dir * 2);
        }
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        lr.enabled = false;
        ballManager.Shoot(dir);
    }

    public void OnDrag(PointerEventData eventData)
    {
        dir = ((Vector2)mainCam.ScreenToWorldPoint(eventData.position) - first).normalized;        
        lr.SetPosition(1, first + dir * 2);
    }
}
