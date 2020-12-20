using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuantityUp : Brick
{
    protected override void Destroy()
    {
        brickManager.quantityUpCount++;
        base.Destroy();
    }
}
