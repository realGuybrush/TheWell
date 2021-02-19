using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerControls : BasicMovement
{
    public int chosenSlot = 0;
    public bool placing = false;
    public GameObject placedObject;
    public void ActWithChosenItem()
    {
        if (items[chosenSlot])
        {
            switch (chosenSlot)
            {
                case 0:
                    SwingPickaxe();
                    break;
                case 1:
                    Shoot();
                    break;
                case 2:
                    PlaceLadder();
                    break;
                case 3:

                    break;
                default:
                    break;
            }
        }
    }

    public void StopActing()
    {
        GameObject.Destroy(placedObject);
        placing = false;
    }
    public void SwingPickaxe()
    {
    }
    public void Shoot()
    {
        //call GameObject.Instantiate from WorldManager 
        //GameObject bullet = GameObject.Instantiate(itemValues.GetProjectile(atkType), projectilePosition, Quaternion.identity, GameObject.Find("Projectiles").transform);
        //bullet.GetComponent<Projectile>().ignore = GameObject.Find(masterName);
        //bullet.transform.right = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x - projectilePosition1.x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y - projectilePosition1.y);
        //bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.right * projectileVelocity;
    }

    public void UpdatePlacingObject()
    {
        if (placing)
        {
            CanPlaceLadder();
            placedObject.transform.position = GetMousePositionInRange(pickingDistance);
        }
    }

    public bool CanPlaceLadder()
    {
        Vector2 colliderSize = placedObject.GetComponent<BoxCollider2D>().size;
        Vector2 pos1 = new Vector2(placedObject.transform.position.x, placedObject.transform.position.y + colliderSize.y / 2) * placedObject.transform.up;
        Vector2 pos2 = new Vector2(placedObject.transform.position.x, placedObject.transform.position.y - colliderSize.y / 2) * placedObject.transform.up;
        Vector2 size = new Vector2(colliderSize.x, 0.01f);
        RaycastHit2D[] rayCastUp = Physics2D.BoxCastAll(pos1, size, 0.0f, placedObject.transform.up, 0.05f, landLayer + platformLayer);
        RaycastHit2D[] rayCastDown = Physics2D.BoxCastAll(pos2, size, 0.0f, placedObject.transform.up * -1.0f, 0.05f, landLayer + platformLayer);
        RaycastHit2D[] rayCastMiddle = Physics2D.BoxCastAll(placedObject.transform.position, colliderSize, 0.0f, placedObject.transform.up, 0.0f, landLayer + platformLayer);
        for (int i = 0; i < rayCastMiddle.Length; i++)
        {
            if (!rayCastMiddle[i].collider.gameObject.name.Contains("Ladder"))
            {
                GlobalFuncs.SetColor(placedObject, Color.red);
                return false;
            }
        }
        for (int i = 0; i < rayCastUp.Length; i++)
        {
            if (!rayCastUp[i].collider.gameObject.name.Contains("Ladder"))
            {
                GlobalFuncs.SetColor(placedObject, Color.green);
                return true;
            }
        }
        for (int i = 0; i < rayCastDown.Length; i++)
        {
            if (!rayCastDown[i].collider.gameObject.name.Contains("Ladder"))
            {
                GlobalFuncs.SetColor(placedObject, Color.green);
                return true;
            }
        }
        GlobalFuncs.SetColor(placedObject, Color.white);
        return true;
    }

    public void PlaceLadder()
    {
        if (!placing)
        {
            placedObject = GameObject.Instantiate((GameObject)Resources.Load("Prefabs\\Ladder"), GetMousePositionInRange(pickingDistance), Quaternion.identity);
            placedObject.GetComponent<BoxCollider2D>().enabled = false;
            GlobalFuncs.SetTransparency(placedObject, 0.5f);
            placing = true;
        }
        else
        {
            if (CanPlaceLadder())
            {
                placedObject.GetComponent<BoxCollider2D>().enabled = true;
                GlobalFuncs.SetColor(placedObject, Color.white);
                GlobalFuncs.SetTransparency(placedObject, 1.0f);
                items[2] = false;
                placing = false;
            }
        }
    }
}
