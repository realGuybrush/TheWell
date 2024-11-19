using UnityEngine;

public class PlayerControls : BasicMovement
{
    ControlKeys keys;
    [SerializeField]
    private CameraMovement myCamera;
    protected override void Awaking()
    {
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
        FollowCursor();
        CheckNumbersInput();
        PlayerCheckMove();
        CheckFallPlatformInput();
        if (!IsFallingFromPlatform && !IsClimbing)
            CheckJumpInput();
        CheckAtkInput();
        CheckActionInput();
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

    private void FollowCursor()
    {
        targetPoint = myCamera.ScreenToWorldPoint(Input.mousePosition);
    }

    private void CheckCrawlInput()
    {
        if (Input.GetKeyDown(keys.Crawl))
        {
            Crawl();
        }
    }
    private void CheckAtkInput()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            MeleeAtk(transform.position, targetPoint);
        }

        if (Input.GetButtonUp("Fire2"))
        {
            RangedAtk(transform.position, targetPoint);
        }
    }

    private void CheckNumbersInput()
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
}
