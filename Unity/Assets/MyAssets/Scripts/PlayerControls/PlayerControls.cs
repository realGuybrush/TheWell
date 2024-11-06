using System.Collections;
using UnityEngine;

public class PlayerControls : BasicMovement
{
    ControlKeys keys;
    [SerializeField]
    private CameraMovement myCamera;
    protected override void Awaking() {
        base.Awaking();
        if(myCamera != null)
            Climbing += myCamera.StartSmoothMovement;
    }

    protected override void Starting()
    {
        base.Starting();
        keys = WorldManager.Instance.AllControlKeys;
    }

    protected override void Updating()
    {
        base.Updating();
        //UpdatePlacingObject();
        CheckNumbersInput();
        CheckCursorAngle();
        PlayerCheckMove();
        //if (!IsClimbing())
        {
            CheckRotateInput();
            CheckScrollInput();
            PlayerCheckMove();
            CheckFallPlatformInput();
            if (!IsFallingFromPlatform && !IsClimbing)
                CheckJumpInput();
            //CheckAtkInput();
            CheckActionInput();
        }
        CheckEsc();
    }

    private void PlayerCheckMove()
    {
        CheckRunInput();
        CheckCrawlInput();
        Vector2 movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (!GlobalFuncs.AroundZero(movement.x) || !GlobalFuncs.AroundZero(movement.y)) {
            HandleMoveCommand(movement);
        }
    }private void CheckFallPlatformInput()
    {
        if (Input.GetButtonDown("Jump") && (Input.GetKey(keys.MoveDown) || Input.GetKey(keys.MoveDown2)))
        {
            FallFromPlatform();
        }
    }

    private void CheckJumpInput()
    {
        if (Input.GetButton("Jump"))
        {
            Jump();
        }
    }

    private void CheckRunInput()
    {
        if (Input.GetKeyDown(keys.Run))
        {
            Run();
        }

        if (Input.GetKeyUp(keys.Run))
        {
            UnRun();
        }
    }

    private void CheckActionInput()
    {
        if (Input.GetKeyDown(keys.Action))
        {
            if (PickUp())
                return;
            ActWithChosenItem();
        }
    }


    private void CheckRotateInput()
    {
        //if (placing)
        {
            //if (Input.GetKeyDown(KeyCode.R))
            {
                //placedObject.transform.localEulerAngles += new Vector3(0.0f, 0.0f, -90.0f);
            }
        }
    }

    private void CheckScrollInput()
    {
        /*if (Input.mouseScrollDelta.y > 0.0f)
        {
            ProtrudeLadder();
        }
        if (Input.mouseScrollDelta.y < 0.0f)
        {
            RetrudeLadder();
        }*/
    }

    private void CheckCrawlInput()
    {
        if (Input.GetKeyDown(keys.Crawl))
        {
            Crawl();
        }
    }
    public void CheckAtkInput()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }

        if (Input.GetButtonUp("Fire1"))
        {
            //BasicAtk1(false, GetAttackType(1), GetBuff(1));
        }
    }

    public void StopAttacking()
    {
        //BasicAtk1(false, GetAttackType(1), GetBuff(1));
        /*fix anim.SetVar("Moving", false);*/
    }

    public void CheckDirections()
    {
        if (Input.GetKeyDown(keys.MoveUp) || Input.GetKeyDown(keys.MoveUp2))
        {
            //BasicSetUp(true);
        }

        if (Input.GetKeyUp(keys.MoveUp) || Input.GetKeyUp(keys.MoveUp2))
        {
            //BasicSetUp(false);
        }

        if (Input.GetKeyDown(keys.MoveDown) || Input.GetKeyDown(keys.MoveDown2))
        {
            //BasicSetDown(true);
        }

        if (Input.GetKeyUp(keys.MoveDown) || Input.GetKeyUp(keys.MoveDown2))
        {
            //BasicSetDown(false);
        }

        if (Input.GetKeyDown(keys.MoveRight) || Input.GetKeyDown(keys.MoveRight2))
        {
            //BasicSetRight(true);
        }

        if (Input.GetKeyUp(keys.MoveRight) || Input.GetKeyUp(keys.MoveRight2))
        {
            //BasicSetRight(false);
        }

        if (Input.GetKeyDown(keys.MoveLeft) || Input.GetKeyDown(keys.MoveLeft2))
        {
            //BasicSetLeft(true);
        }

        if (Input.GetKeyUp(keys.MoveLeft) || Input.GetKeyUp(keys.MoveLeft2))
        {
            //BasicSetLeft(false);
        }
    }

    void CheckNumbersInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            inventory.SelectItem(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            inventory.SelectItem(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            inventory.SelectItem(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            inventory.SelectItem(3);
        }
    }
    void CheckEsc()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

        }
    }

    public void CheckCursorAngle()
    {
        //anim.SetVar("Aim", GetMouseAngleWithFlip(GetCenterOfShootPartRotation()));
    }

    protected void Shoot()
    {
        StartCoroutine("StopAiming");
        //anim.SetVar("Aiming", true);
        //base.Shoot();
    }
    public IEnumerator StopAiming()
    {
        yield return new WaitForSeconds(2.5f);
        //anim.SetVar("Aiming", false);
    }

}
