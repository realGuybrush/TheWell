using System;
using System.Collections;
using System.Collections.Generic;
using MyAssets.Scripts.Inventory_Items;
using UnityEngine;

[Serializable]
public class BasicMovement : EnvInteractor {

    private Animations animations = new Animations();

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private Health health = new Health();

    [SerializeField]
    protected Inventory inventory;

    [SerializeField]
    private bool canWalk = true,
                 canJump = true,
                 canRun = true,
                 canCrawl = true,
                 canClimb = true,
                 canFly = false;

    [SerializeField]
    private float movementMultiplier = 1.0f;
    [SerializeField]
    private float baseWalkSpeed = 3.0f;
    [SerializeField]
    private float slowDownTime = 0.1f;
    private float slowDownTimer;

    [SerializeField]
    private float runMultiplier = 2.0f;
    private bool running;

    [SerializeField]
    private float crawlingMultiplier = 0.5f;
    private bool crawling;
    private bool switchingCrawlStance;
    private float switchCrawlStanceTime = 0.5f;

    public bool attacking;

    [SerializeField]
    private float jumpSpeedY = 5.0f;

    [SerializeField]
    private Transform checkPos;
    private float slopeCheckDistanceDown = 0.3f;
    private RaycastHit2D rayDown;
    private bool onSlope;
    [SerializeField]
    private float addedForceDefault = 5f;

    [SerializeField]
    private float climbXChange = 0.3f;
    [SerializeField]
    private float climbYChange = 0.6f;
    [SerializeField]
    private float climbingTime = 0.9f;
    [SerializeField]
    private float climbingAnimationDelay = 0.6f;
    private bool grabbing;
    private float grabbingTime = 0.3f;
    private bool hanging;
    private bool climbing;
    [SerializeField]
    private int holdingMaximumTime = 10000;
    private int landTimer;
    public event Action<Vector2, float, float> Climbing = delegate {  };

    private int onThisManyClimbableObjects = 0;
    [SerializeField]
    private float climbableClimbingYModifier = 0.2f;

    private bool fallingFromPlatform;
    [SerializeField]
    private float fallingTime = 0.5f;

    private List<Item> pickableItems = new List<Item>();

    [SerializeField]
    private Transform weaponHolder;
    private Weapon spawnedWeapon;
    [SerializeField]
    private float weaponDespawnTime = 5f;
    private float weaponDespawnTimer;
    protected Vector3 targetPoint;
    [SerializeField]
    private Transform defaultTargetPoint;

    [SerializeField]
    private Transform shoulderJointPoint;

    protected override void Awaking()
    {
        base.Awaking();
        animations.InitAnimator(animator);
        animator.GetBehaviour<ClimbAnimationBehaviour>().endClimbing += EndClimbing;
        animator.GetBehaviour<ClimbToCrawlAnimationBehaviour>().endClimbing += EndClimbing;
        OnLanding += StopFlying;
        inventory.InitInventory();

    }

    protected override void Updating()
    {
        base.Updating();
        BasicHandleMidAir();
        CheckIfOnSlope();
        if (onSlope)
            StayOnSlope();
        UpdateHang();
        SlowDown();
        UpdateWeaponTimer();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(fallingFromPlatform)
            if(IsItPlatform(other))
                StartCoroutine("FallFromThisPlatform", other);
    }
    private void OnCollisionStay2D(Collision2D other)
    {
        if(fallingFromPlatform)
            if(IsItPlatform(other))
                StartCoroutine("FallFromThisPlatform", other);
    }

    private void BasicHandleMidAir() {
        if (!(IsLanded() || hanging || IsAirborne))
        {
            animations.SetVar("MidAir", true);
            UnCrawl();
        } else
        {
            animations.SetVar("MidAir", false);
        }
    }

    ///////MOVE///////
    public void HandleMoveCommand(Vector2 direction)
    {
        if (direction.x != 0 || direction.y != 0)
        {
            if (!IsLanded() && canClimb &&
                (IsAgainstLedge() || IsOnLedge() && hanging))
            {
                if(!grabbing)
                {
                    if (hanging)
                        Climb(direction.x);
                    else
                        if (DirectionMatchesFacing(direction.x))
                            Hang();
                }
            }
            else
            {
                MoveInDirection(direction);
            }
        }
    }

    private void MoveInDirection(Vector2 direction)
    {
        if (!canWalk) return;
        Move(direction);
        HandleFlip(direction.x);
    }

    private void Move(Vector2 direction)
    {
        MoveX(direction.x);
        MoveY(direction.y);
    }

    private void MoveX(float directionX)
    {
        if (directionX == 0) return;
        float walkSpeed = baseWalkSpeed * movementMultiplier;
        if (running) walkSpeed *= runMultiplier;
        if (crawling) walkSpeed *= crawlingMultiplier;
        thisObject.velocity = new Vector2(walkSpeed * directionX,
            thisObject.velocity.y);
        animations.SetVar("Moving", true);
        slowDownTimer = slowDownTime;
    }

    private void MoveY(float directionY)
    {
        if (directionY == 0) return;
        float ascensionSpeed = baseWalkSpeed * movementMultiplier;
        float additionalXModifier = 1f;
        if (!canFly)
        {
            if (onThisManyClimbableObjects > 0)
            {
                ascensionSpeed *= climbableClimbingYModifier;
                additionalXModifier = climbableClimbingYModifier;
                SetKinematic(true);
                //animations.SetVar("LadderClimbing", true);
            } else
            {
                ascensionSpeed = 0f;
            }
        } else
        {
            if (IsLanded())
            {
                LiftOff();
                //animations.SetVar("Flying", true);
            }
        }
        if(ascensionSpeed != 0)
            thisObject.velocity = new Vector2(thisObject.velocity.x * additionalXModifier,
                ascensionSpeed * directionY);
        slowDownTimer = slowDownTime;
        //fix: there should be y part on slopes too
    }

    private void SlowDown()
    {
        if (slowDownTimer > 0)
        {
            slowDownTimer -= Time.deltaTime;
            if(slowDownTimer <= 0)
            {
                animations.SetVar("Moving", false);
                //if(onThisManyClimbableObjects > 0 || IsAirborne)
                    thisObject.velocity = Vector2.zero;
            }
        }
        else{
            if(IsLanded())
                thisObject.velocity = Vector2.zero;
        }
    }

    private void StopFlying()
    {
        //animations.SetVar("Flying", false);
    }

    ///////SLOPES///////
    private void CheckIfOnSlope()
    {
        rayDown = Physics2D.Raycast(checkPos.position, Vector2.down, slopeCheckDistanceDown, WhatIsGround);
        onSlope = !GlobalFuncs.AroundZero(rayDown.normal.x);
    }

    private void StayOnSlope()
    {
        //I won't delete it, because I'll have to rediscover everything next time otherwise.
        //Vector2 slopeNormalPerp = Vector2.Perpendicular(rayDown.normal);
        //Debug.DrawRay(checkPos.position, -rayDown.normal, Color.green);//perpendicular through slope
        //Debug.DrawRay(checkPos.position, slopeNormalPerp, Color.red);//left parallel slope
        //Debug.DrawRay(checkPos.position, -slopeNormalPerp, Color.yellow);//right parallel slope
        //Debug.DrawRay(checkPos.position, Vector2.down*slopeCheckDistanceDown, Color.blue);//down
        thisObject.AddForce(-rayDown.normal.normalized * addedForceDefault);
    }

    ///////RUN & CRAWL///////
    public void Run()
    {
        if (canRun && !crawling)
        {
            running = true;
            animations.SetVar("Run", running);
        }
    }

    public void UnRun()
    {
        running = false;
        animations.SetVar("Run", running);
    }

    public void Crawl()
    {
        if (switchingCrawlStance) return;
        if(!hanging)
        {
            if (canCrawl && !running)
            {
                crawling = !crawling;
                animations.SetVar("Crawl", crawling);
                StartCoroutine("SwitchCrawlStanceCoroutine");
            }
        }
        else
            UnHang();
    }

    private void LedgeCrawl()
    {
        animations.SetVar("Crawl", true);
        crawling = true;
    }

    private void UnCrawl()
    {
        if (switchingCrawlStance) return;
        crawling = false;
        animations.SetVar("Crawl", crawling);
        StartCoroutine("SwitchCrawlStanceCoroutine");
    }

    private IEnumerator SwitchCrawlStanceCoroutine()
    {
        switchingCrawlStance = true;
        yield return new WaitForSeconds(switchCrawlStanceTime);
        switchingCrawlStance = false;
    }

    ///////JUMP///////
    public void Jump()
    {
        if (!canJump || switchingCrawlStance) return;
        if (crawling)
        {
            UnCrawl();
            return;
        }
        if (IsLanded() || IsOnLedge() && hanging)
        {
            onSlope = false;
            Vector2 jumpVector = new Vector2(thisObject.velocity.x, jumpSpeedY);
            thisObject.velocity = jumpVector;
            animations.SetVar("MidAir", true);
            UnHang();
        }
    }

    ///////LEDGE GRAB & CLIMB///////
    private void Hang()
    {
        grabbing = true;
        landTimer = holdingMaximumTime;
        hanging = true;
        SetKinematic(true);
        thisObject.transform.position += CalculateHangingPosY();
        thisObject.velocity = Vector2.zero;
        animations.SetVar("Grab", true);
        StartCoroutine("GrabbingCoroutine");
    }

    public IEnumerator GrabbingCoroutine()
    {
        yield return new WaitForSeconds(grabbingTime);
        grabbing = false;
    }

    private void Climb(float directionX)
    {
        if(DirectionMatchesFacing(directionX))
        {
            if(!CanClimbInStanding())
            {
                if (CanClimbInCrawling())
                {
                    LedgeCrawl();
                }
                else
                    return;
            }
            climbing = true;
            animations.SetVar("Climb", true);
            Climbing?.Invoke(new Vector2(climbXChange * FacingRightFloat, climbYChange), climbingTime, climbingAnimationDelay);
        }
    }

    private void EndClimbing()
    {
        UnHang();
        transform.position += new Vector3(FacingRightFloat * climbXChange, climbYChange);
        ProcessEnvCheckersCollisions();
        climbing = false;
    }

    private void UnHang()
    {
        SetKinematic(false);
        landTimer = 0;
        hanging = false;
        animations.SetVar("Grab", false);
    }

    private void UpdateHang()
    {
        if (hanging)
        {
            if (landTimer == 0)
            {
                UnHang();
            }
            landTimer--;
        }
    }

    public bool IsClimbing => climbing;

    ///////LADDERS AND SUCH///////

    public void SteppedOnClimbable()
    {
        onThisManyClimbableObjects++;
    }

    public void SteppedOffClimbable()
    {
        onThisManyClimbableObjects--;
        if(onThisManyClimbableObjects == 0)
            SetKinematic(false);
    }

    ///////PLATFORMS///////
    public void FallFromPlatform()
    {
        fallingFromPlatform = true;
        StartCoroutine("FallFromPlatformCoroutine");
    }

    public IEnumerator FallFromPlatformCoroutine()
    {
        fallingFromPlatform = true;
        yield return new WaitForSeconds(fallingTime);
        fallingFromPlatform = false;
    }

    public IEnumerator FallFromThisPlatform(Collision2D other)
    {
        Collider2D otherCollider = other.collider;
        Physics2D.IgnoreCollision(otherCollider, thisCollider, true);
        yield return new WaitForSeconds(fallingTime);
        Physics2D.IgnoreCollision(otherCollider, thisCollider, false);
    }

    public bool IsFallingFromPlatform => fallingFromPlatform;

    ///////PICKING UP///////

    public void IncludePickable(Item newP)
    {
        pickableItems.Add(newP);
    }

    public void ExcludePickable(Item ExcP)
    {
        if (pickableItems.Contains(ExcP))
            pickableItems.Remove(ExcP);
    }

    public bool PickUp(Item specifiedItem = null)
    {
        ClearEmptyItems();
        if (pickableItems.Count == 0)
            return false;
        int itemIndex = ChooseItemIndex(specifiedItem);
        if (itemIndex == -1) return false;
        //anim.SetVar("PickUp", true);
        if (!inventory.TryToAddItem(pickableItems[itemIndex].GetComponent<Item>()))
            return false;
        Destroy(pickableItems[itemIndex].gameObject);
        return true;
    }

    private void ClearEmptyItems()
    {
        //absolutely needed: without it item[0] will sometimes be null after picking up previous item, and won't be deleted for some reason
        //if you simply put item.RemoveAt(0) after deletion of item, however, two items will be removed from List! magic
        while (pickableItems[0] == null)
            pickableItems.RemoveAt(0);
    }

    private int ChooseItemIndex(Item specifiedItem)
    {
        if (specifiedItem != null)
        {
            for (int i=0; i<pickableItems.Count; i++)
                if (specifiedItem.Index == pickableItems[i].Index)
                    return i;
        }
        else
            return 0;
        return -1;
    }

    ///////ACTIONS AND ATTACKS///////

    public void ActWithChosenItem()
    {
    }

    public void AttackWithWhateverType(Vector2 center, Vector2 target)
    {
        if(inventory.SelectedRanged.Index != -1)
            RangedAtk(center, target);
        else
            if(inventory.SelectedMelee.Index != -1)
                MeleeAtk(center, target);
    }

    public void MeleeAtk(Vector2 center, Vector2 target)
    {
        if (crawling || hanging || climbing || !IsLanded()) return;
        if (spawnedWeapon == null || spawnedWeapon.itemType == ItemType.rangedWeapon)
        {
            Item trySpawnWeapon = inventory.SelectedMelee;
            if (trySpawnWeapon == null) return;
            spawnedWeapon = trySpawnWeapon.GetComponent<Weapon>();
        }
        Strike(center, target);
    }

    private void Strike(Vector2 center, Vector2 target)
    {
        animations.SetVar("Melee", true);
        spawnedWeapon.transform.parent = weaponHolder;
        spawnedWeapon.transform.localPosition = Vector3.zero;
        weaponDespawnTimer = weaponDespawnTime;
        spawnedWeapon.Attack(gameObject, GetAttackingDirection(center, target));
    }

    public void RangedAtk(Vector2 center, Vector2 target)
    {
        if (crawling || hanging || climbing || !IsLanded()) return;
        if (spawnedWeapon == null || spawnedWeapon.itemType == ItemType.meleeWeapon)
        {
            Item trySpawnWeapon = inventory.SelectedRanged;
            if (trySpawnWeapon == null) return;
            spawnedWeapon = trySpawnWeapon.GetComponent<Weapon>();
            if(!FacingRight)
                spawnedWeapon.transform.Rotate(rotateAroundY);
        }
        //bullets in this game are in weapon amount, change for other games, if you like
        if (spawnedWeapon.Amount > 0)
            Shoot(center, target);
    }

    private void Shoot(Vector2 center, Vector2 target)
    {
        animations.SetVar("Ranged", true);
        animations.SetVar("Aim", GetMouseAngleWithFlip(shoulderJointPoint.position));
        weaponDespawnTimer = weaponDespawnTime;
        spawnedWeapon.transform.parent = weaponHolder;
        spawnedWeapon.transform.localPosition = Vector3.zero;
        StartCoroutine("ShootingCoroutine", GetAttackingDirection(center, target));
        inventory.RemoveOneBullet();
    }

    private IEnumerator ShootingCoroutine(Vector3 attackingDirection)
    {
        yield return new WaitForSeconds(0.05f);
        spawnedWeapon.Attack(gameObject, attackingDirection);
    }

    private Vector3 GetAttackingDirection(Vector2 centerPos, Vector2 targetPos)
    {
        return new Vector3(targetPos.x - centerPos.x, targetPos.y - centerPos.y);
    }

    private void UpdateWeaponTimer()
    {
        if (weaponDespawnTimer > 0)
        {
            weaponDespawnTimer -= Time.deltaTime;
            animations.SetVar("Aim", GetMouseAngleWithFlip(shoulderJointPoint.position));
            RemoveWeaponAtTimerEnd();
        }
    }

    public float GetMouseAngleWithFlip(Vector2 center)
    {
        float returnAngle = GetMouseAngleRespToCenter(center);
        if (!FacingRight)
            returnAngle = 180.0f - returnAngle;
        if (returnAngle < 0)
            returnAngle += 360.0f;
        return returnAngle;
    }

    private void RemoveWeaponAtTimerEnd()
    {
        if (weaponDespawnTimer <= 0 || crawling || hanging || climbing || !IsLanded())
        {
            weaponDespawnTimer = 0;
            Destroy(spawnedWeapon.gameObject);
            spawnedWeapon = null;
            animations.SetVar("Melee", false);
            animations.SetVar("Ranged", false);
        }
    }

    public float GetMouseAngleRespToCenter(Vector2 center)
    {
        float distance = GlobalFuncs.Distance2D(center, targetPoint);
        float lowerPart = (targetPoint.y - center.y) < 0.0f?1.0f:0.0f;
        float arccos = Mathf.Rad2Deg* Mathf.Acos((targetPoint.x - center.x) / distance);
        return (targetPoint.y - center.y) < 0.0f ? 180.0f + (180.0f - arccos):arccos;
    }

    ///////HEALTH///////

    public float GetDamaged(float incomingDamage)
    {
        return health.GetDamage(incomingDamage);
    }

    public bool IsAlive => health.health > 0;


    ///////SAVE&LOAD///////

    public void BasicLoadData(SaveData saveData)
    {
        transform.position = saveData.position.ToV3();
        transform.eulerAngles = saveData.rotation.ToV3();
        //thisObject.velocity = saveData.velocity.ToV3();
        health.LoadValues(saveData.damage, saveData.defense, saveData.health);
        if (transform.eulerAngles.y != 0)
        {
            //facingRight = false;
        }
    }
}
