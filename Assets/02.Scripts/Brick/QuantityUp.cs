using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Fix
public class QuantityUp : Brick
{
    protected override void Destroy()
    {
        brickManager.quantityUpCount++;
        base.Destroy();
    }
}
