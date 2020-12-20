using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeManager : MonoBehaviour
{
    public enum BigMode { TITLE, LOBBY, GAME };
    public enum SmallMode { NONE, CONNECTINGSERVER, INSERVER, FINDINGMATCH, READYFORGAME, READYFORSHOOT, SHOOT, NEXTLEVEL, WIN, LOSE };

    public AMode titleMode;
    public AMode lobbyMode;
    public AMode gameMode;
    private Dictionary<BigMode, AMode> bigModeDic = new Dictionary<BigMode, AMode>();
    public BigMode bigMode;
    public SmallMode smallMode = (SmallMode)(-1);

    private void Awake()
    {
        bigModeDic.Add(BigMode.TITLE, titleMode);
        bigModeDic.Add(BigMode.LOBBY, lobbyMode);
        bigModeDic.Add(BigMode.GAME, gameMode);
    }

    private void Start()
    {
        titleMode.SetActive(true);
        bigMode = BigMode.TITLE;
        SetMode(BigMode.TITLE, SmallMode.NONE);
    }

    private void SetMode(BigMode bigMode)
    {
        if (!bigModeDic.ContainsKey(bigMode))
        {
            Debug.LogError("wrong mode");
            return;
        }

        bigModeDic[this.bigMode].SetActive(false);
        bigModeDic[bigMode].SetActive(true);

        this.bigMode = bigMode;
    }

    public void SetMode(BigMode bigMode, SmallMode smallMode)
    {
        if (!bigModeDic.ContainsKey(bigMode))
        {
            Debug.LogError("wrong bigMode");
            return;
        }

        if (this.bigMode != bigMode) SetMode(bigMode);

        if (this.smallMode != smallMode)
        {
            bigModeDic[bigMode].SetMode(smallMode);
            this.smallMode = smallMode;
        }
    }
}
