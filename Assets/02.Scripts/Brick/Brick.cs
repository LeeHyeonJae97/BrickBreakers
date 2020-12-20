using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Brick : MonoBehaviour
{
    public enum State { IDLE, MOVEDOWN, DESTROY };

    public PhotonView pv;

    public AudioManager audioManager;
    public BrickManager brickManager;

    public Text lifeText;
    public string keyName;
    private State state;
    private Vector2 targetPos;
    private float velocity;

    private int life;
    public int Life
    {
        get { return life; }

        set
        {
            life = value;
            if (life <= 0) SetState(State.DESTROY);
            else lifeText.text = life.ToString();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((GameManager.isSinglePlay || pv.IsMine) && collision.gameObject.CompareTag("Ball")) Hit();
    }

    private void Update()
    {
        if (GameManager.isSinglePlay || pv.IsMine)
        {
            switch (state)
            {
                case State.IDLE:
                    break;
                case State.MOVEDOWN:
                    if (Move(targetPos)) SetState(State.IDLE);
                    break;
                case State.DESTROY:
                    break;
            }
        }
    }

    public void Initialize(BrickManager brickManager, AudioManager audioManager, int life)
    {
        this.brickManager = brickManager;
        this.audioManager = audioManager;
        Life = life;
        state = State.IDLE;
    }

    public void SetState(State state)
    {
        if (this.state == state) return;

        switch (state) {
            case State.IDLE:
                break;
            case State.MOVEDOWN:
                break;
            case State.DESTROY:
                Destroy();
                break;
        }
        this.state = state;
    }

    public void Hit()
    {
        Life -= 1;
        audioManager.PlayVfx("Hit");
    }

    protected virtual void Destroy() => brickManager.Destroy(gameObject);

    public void MoveDown(float dist)
    {
        targetPos = (Vector2)transform.position + Vector2.down * dist;
        velocity = dist / 0.5f;
        SetState(State.MOVEDOWN);
    }

    private bool Move(Vector2 targetPos)
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPos, velocity * Time.deltaTime);
        if (((Vector2)transform.position - targetPos).sqrMagnitude < 0.01f)
        {
            transform.position = targetPos;
            return true;
        }
        else return false;
    }
}
