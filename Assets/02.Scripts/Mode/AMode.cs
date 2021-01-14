using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Fix
public abstract class AMode : MonoBehaviour
{
    public GameObject holder;
    public Text status;

    public abstract void SetActive(bool value);
    public abstract void SetMode(ModeManager.SmallMode mode);
}
