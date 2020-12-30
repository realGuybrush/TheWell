using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerControls : BasicMovement
{
    public void Shoot()
    {
        //call GameObject.Instantiate from WorldManager 
        //GameObject bullet = GameObject.Instantiate(itemValues.GetProjectile(atkType), projectilePosition, Quaternion.identity, GameObject.Find("Projectiles").transform);
        //bullet.GetComponent<Projectile>().ignore = GameObject.Find(masterName);
        //bullet.transform.right = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x - projectilePosition1.x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y - projectilePosition1.y);
        //bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.right * projectileVelocity;
    }
}
