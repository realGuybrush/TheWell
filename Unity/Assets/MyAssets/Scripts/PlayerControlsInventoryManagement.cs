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
            placedObject.transform.position = GetMousePositionInRange(pickingDistance);
        }
    }

    public void PlaceLadder()
    {
        if (!placing)
        {
            placedObject = GameObject.Instantiate((GameObject)Resources.Load("Prefabs\\Ladder"), GetMousePositionInRange(pickingDistance), Quaternion.identity);
            placedObject.GetComponent<BoxCollider2D>().enabled = false;
            placing = true;
        }
        else
        {
            placedObject.GetComponent<BoxCollider2D>().enabled = true;
            items[2] = false;
            placing = false;
        }
    }
}
