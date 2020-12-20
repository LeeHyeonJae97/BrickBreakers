using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUp : Brick
{
    protected override void Destroy()
    {
        brickManager.speedUpCount++;
        base.Destroy();
    }
}
