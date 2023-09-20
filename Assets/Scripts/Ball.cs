using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Ball : MonoBehaviour
{
    private Rigidbody rb;
    private float currentTime;
    private bool smash, invincible;

    public enum BallState
    {
        Prepare,
        Playing,
        Died,
        Finish
    }

    [HideInInspector]
    public BallState ballState = BallState.Prepare;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if(ballState == BallState.Playing)
        {
            if(Input.GetMouseButtonDown(0))
                smash = true;

            if(Input.GetMouseButtonUp(0))
                smash = false;

            if(invincible)
            {
                currentTime -= Time.deltaTime * .35f;
            }
            else
            {
                if(smash)
                    currentTime += Time.deltaTime * .8f;
                else
                    currentTime -= Time.deltaTime * .5f;
            }

            if(currentTime >= 1)
            {
                currentTime = 1;
                invincible = true;
            }

            else if (currentTime <= 0)
            {
                currentTime = 0;
                invincible = false;
            }
        }

        if(ballState == BallState.Prepare)
        {
            if(Input.GetMouseButtonDown(0))
                ballState = BallState.Playing;
        }

        if (ballState == BallState.Finish)
        {
            if(Input.GetMouseButtonDown(0))
                FindObjectOfType<LevelSpawner>().NextLevel();
        }
    }

    void FixedUpdate()
    {
        if(ballState == BallState.Playing)
        {
            if(Input.GetMouseButton(0))
            {
                smash = true;
                rb.velocity = new Vector3(0,-100 * Time.fixedDeltaTime * 7, 0);
            }
        }

        if(rb.velocity.y > 5)
            rb.velocity = new Vector3(rb.velocity.x, 5, rb.velocity.z);
    }

    public void IncreaseBrokenStacks()
    {
        if(!invincible)
            ScoreManager.instance.AddScore(1);
        else
            ScoreManager.instance.AddScore(2);
    }

    void OnCollisionEnter(Collision target)
    {
        if(!smash)
        {
            rb.velocity = new Vector3(0, 50 * Time.deltaTime * 5, 0);
        }

        else
        {
            if(invincible)
            {
                if(target.gameObject.tag == "enemy" || target.gameObject.tag == "plane")
                {
                    target.transform.parent.GetComponent<StackController>().ShatterAllParts();
                }
            }

            else
            {
                if(target.gameObject.tag == "enemy")
                {
                    target.transform.parent.GetComponent<StackController>().ShatterAllParts();
                }

                if(target.gameObject.tag == "plane")
                {
                    print("Over");
                }
            }
        }

        if(target.gameObject.tag == "Finish" && ballState == BallState.Playing)
        {
            ballState = BallState.Finish;
        }
    }

    void OnCollisionStay(Collision target)
    {
        if(!smash || target.gameObject.tag == "Finish")
        {
            rb.velocity = new Vector3(0, 50 * Time.deltaTime * 5, 0);
        }
    }


}
