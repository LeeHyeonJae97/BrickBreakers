using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BallManager : MonoBehaviour
{
    public ModeManager modeManager;
    public Timer shootTimer;

    public List<GameObject> balls = new List<GameObject>();
    private WaitForSeconds cooldown = new WaitForSeconds(0.05f);
    private bool isGatherPosSettled;
    [HideInInspector] public float power = 5;
    public const float bottomY = -6.47f;

    private Vector2 gatherPos;
    public Vector2 GatherPos
    {
        get { return gatherPos; }

        set
        {
            if (!isGatherPosSettled)
            {
                gatherPos = value;
                isGatherPosSettled = true;
            }
        }
    }
    private int idleCount;
    public int IdleCount
    {
        get { return idleCount; }

        set
        {
            idleCount = value;
            if (idleCount >= balls.Count) modeManager.SetMode(ModeManager.BigMode.GAME, ModeManager.SmallMode.NEXTLEVEL);
        }
    }

    private void Awake()
    {
        shootTimer.timeout = ShootByTimeout;
    }

    public void Init()
    {
        gatherPos = new Vector2(0, bottomY);
        Spawn(gatherPos);
    }

    public void NextLevel(int quantityUpCount, int speedUpCount)
    {
        for (int i = 0; i < quantityUpCount; i++)
            Spawn(gatherPos);

        //power = Mathf.Clamp(power + speedUpCount, 0, 6); //** 네트워크가 속도를 못따라간다

        idleCount = 0;
        isGatherPosSettled = false;
    }

    // Fix
    public void Shoot(Vector2 dir)
    {
        StartCoroutine(CorShoot(dir));

        modeManager.SetMode(ModeManager.BigMode.GAME, ModeManager.SmallMode.SHOOT);
    }

    // Fix
    public void ShootByTimeout()
    {
        float x = Random.Range(-1f, 1f);
        float y = Random.Range(0.1f, 1);

        Vector2 dir = new Vector2(x, y).normalized;

        float angle = Vector2.Angle(dir, Vector2.right);

        if (angle < 20) dir = new Vector2(Mathf.Cos(20 * Mathf.Deg2Rad), Mathf.Sin(20 * Mathf.Deg2Rad));
        else if (angle > 160) dir = new Vector2(Mathf.Cos(160 * Mathf.Deg2Rad), Mathf.Sin(160 * Mathf.Deg2Rad));

        StartCoroutine(CorShoot(dir));

        modeManager.SetMode(ModeManager.BigMode.GAME, ModeManager.SmallMode.SHOOT);
    }

    // Fix
    private IEnumerator CorShoot(Vector2 dir)
    {
        for (int i = 0; i < balls.Count; i++)
        {
            balls[i].GetComponent<Ball>().Shoot(dir);
            yield return cooldown;
        }
    }

    public void Spawn(Vector2 pos)
    {
        GameObject ball = GameManager.isSinglePlay ? PoolManager.instance.Get("Ball", null, pos) :
            PhotonNetwork.Instantiate("Ball_Net", pos, Quaternion.identity);
        ball.SetActive(true);
        ball.GetComponent<Ball>().Initialize(this);

        balls.Add(ball);
    }

    public void GameOver()
    {
        for (int i = 0; i < balls.Count; i++)
        {
            if (GameManager.isSinglePlay) PoolManager.instance.Return(balls[i]);
            else if (balls[i].GetComponent<PhotonView>().IsMine) PhotonNetwork.Destroy(balls[i]);            
        }            

        balls.Clear();
    }
}
