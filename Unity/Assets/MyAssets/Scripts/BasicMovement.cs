using UnityEngine;

public partial class BasicMovement : MonoBehaviour
{

    public Animations anim = new Animations();
    public Health thisHealth;
    public Rigidbody2D thisObject;

    public bool attacking = false;

    private void Start()
    {
        InitValues();
    }

    virtual public void InitValues()
    {
        DefineGroundLayer();
        thisCollider = gameObject.GetComponent<CapsuleCollider2D>();
        thisObject = gameObject.GetComponent<Rigidbody2D>();
        thisHealth = thisObject.GetComponent<Health>();
        anim.a = GetComponent<Animator>();
        SetLandChecker();
        SetStepChecker();
        SetWallChecker();
        SetLedgeChecker();
    }
    public bool BasicCheckHealth()
    {
        if (thisHealth.values.GetHealth() <= 0)
        {
            return false;
        }
        return true;
    }

    public bool BasicCheckMidAir()
    {
        if (IsMidAir())
        {
            AdjustMidAirFriction();
            anim.SetVar("MidAir", true);
            return true;
        }
        else
        {
            AdjustLandFriction();
            anim.SetVar("MidAir", false);
            return false;
        }
    }
    public bool IsMidAir()
    {
        return !(land.landed || ledge.landed || wall.landed || step.landed);
    }

    public bool IsClimbing()
    {
        return anim.a.GetBool("Climb");
    }

    public void BasicAtk(bool atk, string attackType, Buff buff)
    {
        anim.SetVar(attackType, atk);
        thisHealth.values.attacking = true;
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

    public void BasicLoadData(SVector3 position, SVector3 rotation, SVector3 speed, Characteristics newCharacteristics)
    {
        gameObject.transform.position = position.ToV3();
        gameObject.transform.eulerAngles = rotation.ToV3();
        gameObject.GetComponent<Rigidbody2D>().velocity = speed.ToV3();
        thisHealth.values = newCharacteristics;
        if (transform.eulerAngles.y != 0)
        {
            facingRight = false;
        }
    }
}
