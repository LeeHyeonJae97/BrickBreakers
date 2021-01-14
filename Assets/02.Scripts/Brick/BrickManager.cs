using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class BrickManager : MonoBehaviour
{
    public AudioManager audioManager;

    public Text combo;
    public GameObject comboCanvas;
    public Transform[] spawnPoses;
    public List<GameObject> bricks = new List<GameObject>();
    public int quantityUpCount;
    public int speedUpCount;
    public int attackedAmount;
    private const float height = 0.51f;

    private int destroyCount;
    public int DestroyCount
    {
        get { return destroyCount; }

        set
        {
            destroyCount = value;
            if (destroyCount == 1) comboCanvas.SetActive(true);
            combo.text = destroyCount.ToString();
        }
    }

    public void NextLevel(int level)
    {
        comboCanvas.SetActive(false);

        int speedUp = Random.Range(0, 2);
        int quantityUp = 1;
        int brick = Random.Range(2, 5);
        int total = Mathf.Clamp(speedUp + quantityUp + brick + attackedAmount, 0, 21);

        Vector2[] poses = GetPoses(total);

        if(speedUp > 0) Spawn("SpeedUp", poses[0], 1);
        Spawn("QuantityUp", poses[1], 1);
        for (int i = 2; i < poses.Length; i++) Spawn("Brick", poses[i], level);

        MoveDown(height * ((total / 7) + Mathf.Clamp(total % 7, 0, 1)));

        destroyCount = quantityUpCount = speedUpCount = attackedAmount = 0;
    }

    private void Spawn(string key, Vector2 pos, int life)
    {
        GameObject brick = GameManager.isSinglePlay ? PoolManager.instance.Get(key, null, pos) :
            PhotonNetwork.Instantiate(key + "_Net", pos, Quaternion.identity);
        brick.SetActive(true);
        brick.GetComponent<Brick>().Initialize(this, audioManager, life);

        bricks.Add(brick);
    }

    private Vector2[] GetPoses(int amount)
    {        
        List<Vector2> poses = new List<Vector2>();

        int quotient = amount / 7;
        int remainder = amount % 7;

        int count;
        for (int i = 0; i < quotient; i++)
        {
            count = 0;
            while (count < 7)
            {
                int posIndex = Random.Range(i * 8, (i + 1) * 8);
                if (!poses.Contains(spawnPoses[posIndex].position))
                {
                    poses.Add(spawnPoses[posIndex].position);
                    count++;
                }
            }
        }

        count = 0;
        while (count < remainder)
        {
            int posIndex = Random.Range(quotient * 8, (quotient + 1) * 8);
            if(!poses.Contains(spawnPoses[posIndex].position))
            {
                poses.Add(spawnPoses[posIndex].position);
                count++;
            }
        }
        
        return poses.ToArray();
    }

    public void Destroy(GameObject brick)
    {
        DestroyCount += 1;
        bricks.Remove(brick);
        if (GameManager.isSinglePlay) PoolManager.instance.Return(brick);
        else if(brick.GetComponent<PhotonView>().IsMine) PhotonNetwork.Destroy(brick);        
    }

    // Fix
    private void MoveDown(float dist)
    {
        for (int i = 0; i < bricks.Count; i++)
            bricks[i].GetComponent<Brick>().MoveDown(dist);
    }

    public void GameOver()
    {
        for (int i = 0; i < bricks.Count; i++)
        {
            if (GameManager.isSinglePlay) PoolManager.instance.Return(bricks[i]);
            else if (bricks[i].GetComponent<PhotonView>().IsMine) PhotonNetwork.Destroy(bricks[i]);            
        }

        bricks.Clear();
    }
}
