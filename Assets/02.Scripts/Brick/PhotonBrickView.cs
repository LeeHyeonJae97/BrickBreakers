using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// Fix
public class PhotonBrickView : MonoBehaviourPun, IPunObservable
{
    public Brick brick;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(brick.Life);
        }
        else
        {
            brick.Life = (int)stream.ReceiveNext();
        }
    }
}
