using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Fix
public class SpeedUp : Brick
{
    protected override void Destroy()
    {
        brickManager.speedUpCount++;
        base.Destroy();
    }
}
