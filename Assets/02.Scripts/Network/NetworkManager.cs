using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

// Fix
public class NetworkManager : MonoBehaviourPunCallbacks, IOnEventCallback
{
    public ModeManager modeManager;
    public BrickManager brickManager;

    public override void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    #region TitleScene
    public Text warning;
    public Text nickName;

    public void Connect()
    {
        if (nickName.text.CompareTo("") != 0)
        {
            warning.text = "";
            PhotonNetwork.ConnectUsingSettings();
            modeManager.SetMode(ModeManager.BigMode.TITLE, ModeManager.SmallMode.CONNECTINGSERVER);
        }
        else warning.text = "이름을 입력해야합니다.";
    }
    public override void OnConnectedToMaster()
    {
        //Debug.Log("서버 접속");
        PhotonNetwork.LocalPlayer.NickName = nickName.text;

        modeManager.SetMode(ModeManager.BigMode.LOBBY, ModeManager.SmallMode.INSERVER);
    }

    public void Disconnect() => PhotonNetwork.Disconnect();
    public override void OnDisconnected(DisconnectCause cause)
    {
        //Debug.Log("서버 접속 해제");
        //Debug.Log(cause);
    }
    #endregion

    #region LobbyScene
    public static int roomId = 0;

    public void JoinRandomRoom()
    {
        modeManager.SetMode(ModeManager.BigMode.LOBBY, ModeManager.SmallMode.MAKINGMATCH);

        PhotonNetwork.JoinRandomRoom();
    }
    public override void OnJoinedRoom()
    {
        //Debug.Log("방 입장");
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            modeManager.SetMode(ModeManager.BigMode.LOBBY, ModeManager.SmallMode.FINDINGMATCH);
        else if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            GameManager.isSinglePlay = false;
            modeManager.SetMode(ModeManager.BigMode.GAME, ModeManager.SmallMode.READYFORGAME);
        }
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        //Debug.Log("방 랜덤 입장 실패");
        //Debug.Log(message);

        roomId++;
        PhotonNetwork.CreateRoom(roomId.ToString(), new RoomOptions { MaxPlayers = 2 });
    }

    public void CreateRoom(Text text) => PhotonNetwork.CreateRoom(text.text, new RoomOptions { MaxPlayers = 2 });
    public override void OnCreatedRoom()
    {
        //Debug.Log("방 생성");
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("방 생성 실패");
        Debug.Log(message);

        //joined = false;
        modeManager.SetMode(ModeManager.BigMode.LOBBY, ModeManager.SmallMode.INSERVER);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom()
    {
        //Debug.Log("방 퇴장");

        modeManager.SetMode(ModeManager.BigMode.LOBBY, ModeManager.SmallMode.INSERVER);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        //Debug.Log("상대방 입장");

        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            GameManager.isSinglePlay = false;
            modeManager.SetMode(ModeManager.BigMode.GAME, ModeManager.SmallMode.READYFORGAME);
        }
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1 &&
            modeManager.smallMode != ModeManager.SmallMode.WIN && modeManager.smallMode != ModeManager.SmallMode.LOSE)
        {
            modeManager.SetMode(ModeManager.BigMode.GAME, ModeManager.SmallMode.WIN);
        }
    }
    #endregion

    #region GameScene
    private const byte winEventCode = 1;
    private const byte attackedEventCode = 2;

    public void SendGameOverEvent()
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        object[] content = { ModeManager.BigMode.GAME, ModeManager.SmallMode.WIN };
        PhotonNetwork.RaiseEvent(winEventCode, content, raiseEventOptions, SendOptions.SendReliable);

        modeManager.SetMode(ModeManager.BigMode.GAME, ModeManager.SmallMode.LOSE);
    }

    public void SendAttackedEvent(int amount)
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        PhotonNetwork.RaiseEvent(attackedEventCode, (object)amount, raiseEventOptions, SendOptions.SendReliable);
    }

    public void OnEvent(EventData photonEvent)
    {
        switch (photonEvent.Code)
        {
            case winEventCode:
                {
                    var content = (object[])photonEvent.CustomData;
                    modeManager.SetMode((ModeManager.BigMode)content[0], (ModeManager.SmallMode)content[1]);
                    break;
                }
            case attackedEventCode:
                {
                    brickManager.attackedAmount += (int)photonEvent.CustomData;
                    break;
                }
        }
    }
    #endregion
}
