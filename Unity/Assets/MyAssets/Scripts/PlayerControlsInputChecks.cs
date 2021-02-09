using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerControls : BasicMovement
{
    //ControlKeys keys = new ControlKeys();
    private void CheckMovementDirection()
    {
        movingDirection = Input.GetAxisRaw("Horizontal");
    }

    private void CheckSpeedUp()
    {
        movementMultiplier = Input.GetAxis("Horizontal");
    }
    public bool CheckFallPlatformInput()
    {
        if ((Input.GetButtonDown("Jump") && (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)))||
            ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))&&onLadder))
        {
            bool b = Physics2D.GetIgnoreLayerCollision(0, 11);
            StartCoroutine("FallFromPlatform");
            return true;
        }
        return false;
    }

    private void CheckJumpInput()
    {
        if (Input.GetButton("Jump"))
        {
            if (!BasicCheckHold() && !IsClimbing())
            {
                BasicJump();
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
            if (!crawling)
            {
                anim.SetVar("Run", true);
                Run();
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            anim.SetVar("Run", false);
            UnRun();
        }
    }

    public void CheckActionInput()
    {
        if (Input.GetKeyDown(KeyCode.E)||Input.GetKeyDown(KeyCode.Mouse0))
        {
            ActWithChosenItem();
        }
    }

    public void CheckLadderInput()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            CheckLadderInput();
        }
    }

    public bool CheckPickUpInput()
    {
        if (pickableItem.Count != 0)
        {
            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Mouse0))
            {
                anim.SetVar("PickUp", true);
                PickUp(pickableItem);
                return true;
            }
        }
        anim.SetVar("PickUp", false);
        return false;
    }

    public void CheckCrawlInput()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
                if (!running && !BasicCheckHold())
                {
                    if (!CheckCrawl())
                    {
                        anim.SetVar("Crawl", true);
                        Crawl();
                    }
                    else
                    {
                        anim.SetVar("Crawl", false);
                        UnCrawl();
                    }
                }
                else
                {
                    if (BasicCheckHold())
                    {
                        Unhold();
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

    public void CheckClimbLadder()
    {
        if (onLadder)
        {
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) ||
                Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
            {
                ClimbLadder();
            }
            else
            {
                UndoFalling();
            }
        }
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

    void CheckNumbersInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            chosenSlot = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            chosenSlot = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            chosenSlot = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            chosenSlot = 3;
        }
    }
    void CheckEsc()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

        }
    }
}
