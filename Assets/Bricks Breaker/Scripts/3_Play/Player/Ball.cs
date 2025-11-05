﻿using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Ball : MonoBehaviour
{
    public Rigidbody2D rb;
    [HideInInspector] public bool isFirst = false;
    private float speed = 1f;
    private bool isReset = false;
    public int damage = 1;
    public SpriteRenderer spriteBall;

    // Add this static variable to track all ball instances
    private static int ballLayer = -1;

    void Awake()
    {
        // Initialize the ball layer if not set
        if (ballLayer == -1)
        {
            ballLayer = LayerMask.NameToLayer("Ball");
        }
        
        // Set this ball to the Ball layer
        if (ballLayer != -1)
        {
            gameObject.layer = ballLayer;
        }
    }

    public void SetData(int damage)
    {
        this.damage = damage;
        isReset = false;
        _isDestoryOn = false;
        rb.AddRelativeForce(Player.instance.shotRot.transform.up.normalized * speed, ForceMode2D.Impulse);
        GetComponent<CircleCollider2D>().enabled = true;
    }

    bool _isDestoryOn = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Ignore collisions with other balls
        if (collision.gameObject.CompareTag("Ball"))
        {
            return;
        }
        
        CtrGame.instance.ShotSound();
        _isDestoryOn = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isDestoryOn)
        {
            if (collision.CompareTag("InTrigger"))
            {
                if (!Player.instance.isFirst)
                {
                    Player.instance.isFirst = true;
                    Player.instance.SetNextPositionX(transform.position.x);
                    Reset();
                }
                else
                {
                    MoveBall();
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (_isDestoryOn)
        {
            if (collision.CompareTag("InTrigger"))
            {
                if (!Player.instance.isFirst)
                {
                    Player.instance.isFirst = true;
                    Player.instance.SetNextPositionX(transform.position.x);
                    Reset();
                }
                else
                {
                    MoveBall();
                }
            }
        }
    }

    public void MoveBall()
    {
        GetComponent<CircleCollider2D>().enabled = false;
        rb.velocity = Vector3.zero;
        transform.DOKill();
        transform.DOMove(Player.instance.nextPosition, 0.15f).SetEase(Ease.OutCubic).OnComplete(() => { Reset(); });
    }

    public void ReturnBall()
    {
        rb.velocity = Vector3.zero;
        GetComponent<CircleCollider2D>().enabled = false;
        transform.DOMove(Player.instance.nextPosition, 0.25f).SetEase(Ease.OutCubic).OnComplete(() => { Reset(); });
    }

    private void Reset()
    {
        if (!isReset)
        {
            isReset = true;
            isFirst = false;
            _isDestoryOn = false;
            Player.instance.activeBall.Remove(this);
            PoolManager.Despawn(this.gameObject);
        }
    }
}