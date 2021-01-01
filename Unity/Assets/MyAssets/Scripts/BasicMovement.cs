using UnityEngine;

public class BasicMovement : MonoBehaviour
{
    public Animations anim = new Animations();
    public BasicClimb climb = new BasicClimb();
    public BasicFlip flip = new BasicFlip();
    public BasicJump jump = new BasicJump();
    public BasicLand land;

    public LandCheck landChecker;
    public BasicLand ledge;
    public LandCheck ledgeChecker;

    public BasicMove move = new BasicMove();
    public BasicLand step;
    public LandCheck stepChecker;
    public Health thisHealth;

    public Rigidbody2D thisObject;
    public BasicLand wall;
    public LandCheck wallChecker;

    public bool attacking = false;

    public bool BasicCheckHealth()
    {
        if (thisHealth.values.GetHealth() <= 0)
        {
            return false;
        }
        return true;
    }
    public bool BasicCheckHold()
    {
        var holding = land.holding || ledge.holding || wall.holding || step.holding;
        if (holding)
        {
            anim.SetVar("Grab", holding);
        }
        else
        {
            anim.SetVar("Grab", holding);
        }
        return holding;
    }

    public void CheckLand()
    {
        var touchingLand = !landChecker.FirstJumpSuccessfull();
        var touchingTop = !ledgeChecker.FirstJumpSuccessfull();
        var touchingMid = !wallChecker.FirstJumpSuccessfull();
        var touchingBot = !stepChecker.FirstJumpSuccessfull();
        var onHold = wall.holding || ledge.holding;
        if (!onHold)
        {
            var pressingTowardsWall = move.movingDirection < 0 && !flip.facingRight ||
                                      move.movingDirection > 0 && flip.facingRight;
            land.CheckThisLand(touchingLand);
            if (!land.landed && pressingTowardsWall)
            {
                ledge.CheckThisLand(!touchingTop && touchingMid);
                wall.CheckThisLand(touchingTop && touchingMid && touchingBot);
                step.CheckThisLand(!touchingTop && !touchingMid && touchingBot);
            }
            else
            {
                ledge.landed = false;
                wall.landed = false;
                step.landed = false;
            }
        }
        else
        {
            ledge.CheckThisLand(!touchingTop && touchingMid);
            wall.CheckThisLand(touchingTop && touchingMid && touchingBot);
            step.CheckThisLand(!touchingTop && !touchingMid && touchingBot);
        }
    }

    public void BasicCheckJump(float movingMultiplierX = 0.0f)
    {
        var midAir = !(land.landed || ledge.landed || wall.landed || step.landed);
        var canJumpFromCurrentLand = land.landed && land.canJump || ledge.landed && ledge.canJump ||
                                     wall.landed && wall.canJump || step.landed && step.canJump;
        if (jump.CheckJump(!midAir&&canJumpFromCurrentLand && !move.crawl.crawling, movingMultiplierX))
        {
            anim.SetVar("Jump", true);
            ReleaseHolds();
        }
    }

    public bool IsClimbing()
    {
        Debug.Log(anim.ToString());
        return anim.a.GetBool("Climb");
    }

    public void BasicCheckClimb()
    {
        var holding = land.holding || ledge.holding || wall.holding || step.holding;
        var canClimb = land.holding && land.canClimb || ledge.holding && ledge.canClimb ||
                       wall.holding && wall.canClimb || step.holding && step.canClimb;
        if (holding)
        {
            if (climb.CheckClimb(canClimb))
            {
                anim.SetVar("Climb", true);
                ReleaseHolds();
            }
            else
            {
                anim.SetVar("Climb", false);
            }
        }
        else
        {
            anim.SetVar("Climb", false);
        }
    }

    public void BasicAtk(bool atk, string attackType, Buff buff)
    {
        anim.SetVar(attackType, atk);
        thisHealth.values.attacking = true;
    }

    public bool BasicCheckMidAir()
    {
        if (landChecker.FirstJumpSuccessfull())
        {
            anim.SetVar("MidAir", true);
            return true;
        }
        else
        {
            anim.SetVar("MidAir", false);
            return false;
        }
    }

    public void BasicSetUp(bool value)
    {
        anim.SetVar("Up", value);
    }

    public void BasicSetDown(bool value)
    {
        anim.SetVar("Down", value);
    }

    public void BasicSetRight(bool value)
    {
        anim.SetVar("Right", value);
    }

    public void BasicSetLeft(bool value)
    {
        anim.SetVar("Left", value);
    }

    public void ReleaseHolds()
    {
        land.Unhold();
        ledge.Unhold();
        wall.Unhold();
        step.Unhold();
    }

    public void BasicLoadData(SVector3 position, SVector3 rotation, SVector3 speed, Characteristics newCharacteristics)
    {
        gameObject.transform.position = position.ToV3();
        gameObject.transform.eulerAngles = rotation.ToV3();
        gameObject.GetComponent<Rigidbody2D>().velocity = speed.ToV3();
        thisHealth.values = newCharacteristics;
        if (transform.eulerAngles.y != 0)
        {
            flip.facingRight = false;
        }
    }
}
