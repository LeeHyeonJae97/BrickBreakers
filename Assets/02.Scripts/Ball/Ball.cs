using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Ball : MonoBehaviour
{
    public enum State { IDLE, FLY, GATHER };

    public PhotonView pv;
    public Rigidbody2D rb;
    public CircleCollider2D coll;

    public BallManager ballManager;

    private State state;
    private Vector2 dir;
    private float orgVelocity;

    private void Start()
    {
        if (GameManager.isSinglePlay || pv.IsMine)
        {
            coll.enabled = true;
            InvokeRepeating(nameof(CheckVelocity), 0, 1);
        }

        else coll.enabled = false;
    }

    private void Update()
    {
        if (GameManager.isSinglePlay || pv.IsMine)
        {
            switch (state)
            {
                case State.IDLE:
                    break;
                case State.FLY:
                    if (rb.position.y <= BallManager.bottomY) SetState(State.GATHER);
                    break;
                case State.GATHER:
                    if (Move(ballManager.GatherPos)) SetState(State.IDLE);
                    break;
            }
        }
    }

    public void Initialize(BallManager ballManager)
    {
        this.ballManager = ballManager;
        state = State.IDLE;
    }

    public void SetState(State state)
    {
        if (this.state == state) return;

        switch (state)
        {
            case State.IDLE:
                ballManager.IdleCount += 1;
                break;
            case State.FLY:
                rb.position += Vector2.up * 0.01f;
                rb.AddForce(dir * 7, ForceMode2D.Impulse);
                orgVelocity = rb.velocity.sqrMagnitude;
                break;
            case State.GATHER:
                rb.velocity = Vector2.zero;
                transform.position = new Vector2(transform.position.x, BallManager.bottomY);
                ballManager.GatherPos = transform.position;
                break;
        }
        this.state = state;
    }

    public void Shoot(Vector2 dir)
    {
        this.dir = dir;
        SetState(State.FLY);
    }

    private bool Move(Vector2 targetPos)
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPos, 0.2f);
        if (((Vector2)transform.position - targetPos).sqrMagnitude < 0.01f)
        {
            transform.position = targetPos;
            return true;
        }
        else return false;
    }

    private void CheckVelocity()
    {
        if (rb.velocity.x != 0 && (-0.1f < rb.velocity.y && rb.velocity.y < 0.1f)) rb.velocity = new Vector2(rb.velocity.x, -0.1f);
        if (rb.velocity.sqrMagnitude != orgVelocity) rb.velocity = Mathf.Sqrt(orgVelocity) * rb.velocity.normalized;
    }
}
