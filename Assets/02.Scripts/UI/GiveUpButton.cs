using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveUpButton : MonoBehaviour
{
    public AlertConfirm alertConfirm;
    public NetworkManager networkManager;

    public void OnClick()
    {
        alertConfirm.Confirm("나가시겠습니까?", networkManager.SendGameOverEvent);
    }
}
