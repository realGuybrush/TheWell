using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerControls : BasicMovement
{
    //ControlKeys keys = new ControlKeys();
    private void CheckMovementDirection()
    {
        move.movingDirection = Input.GetAxisRaw("Horizontal");
    }

    private void CheckSpeedUp()
    {
        move.movementMultiplier = Input.GetAxis("Horizontal");
    }

    private void CheckJumpInput()
    {
        if (Input.GetButton("Jump"))
        {
            if (!BasicCheckHold() && !IsClimbing())
            {
                BasicCheckJump();
            }
        }
        else
        {
            anim.SetVar("Jump", false);
        }
    }

    private void CheckClimbInput()
    {
        if (BasicCheckHold() && Input.GetButton("Jump"))
        {
            BasicCheckClimb();
        }
    }

    public void CheckRunInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKey(KeyCode.LeftShift))
        {
            if (!move.crawl.crawling)
            {
                anim.SetVar("Run", true);
                move.run.Run();
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            anim.SetVar("Run", false);
            move.run.UnRun();
        }
    }

    public void CheckActionInput()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            CheckPickUpInput();
        }
    }

    public bool CheckPickUpInput()
    {
        if (pickableItem.Count != 0)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                PickUp(pickableItem);
                return true;
            }
        }
        return false;
    }

    public void CheckCrawlInput()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
                if (!move.run.running && !BasicCheckHold())
                {
                    if (!move.crawl.CheckCrawl())
                    {
                        anim.SetVar("Crawl", true);
                        move.crawl.Crawl();
                    }
                    else
                    {
                        anim.SetVar("Crawl", false);
                        move.crawl.UnCrawl();
                    }
                }
                else
                {
                    if (BasicCheckHold())
                    {
                        ReleaseHolds();
                        anim.SetVar("Grab", false);
                    }
                }
        }
    }
    public void CheckAtkInput()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            //BasicAtk1(true, GetAttackType(1), GetBuff(1));
        }

        if (Input.GetButtonUp("Fire1"))
        {
            //BasicAtk1(false, GetAttackType(1), GetBuff(1));
        }
    }

    public void StopAttacking()
    {
        //BasicAtk1(false, GetAttackType(1), GetBuff(1));
        anim.SetVar("Moving", false);
    }

    public void CheckDirections()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            BasicSetUp(true);
        }

        if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W))
        {
            BasicSetUp(false);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            BasicSetDown(true);
        }

        if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S))
        {
            BasicSetDown(false);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            BasicSetRight(true);
        }

        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D))
        {
            BasicSetRight(false);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            BasicSetLeft(true);
        }

        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A))
        {
            BasicSetLeft(false);
        }
    }
    void CheckEsc()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

        }
    }
}
