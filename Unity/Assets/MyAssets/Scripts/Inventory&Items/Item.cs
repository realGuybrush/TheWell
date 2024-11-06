using System;
using MyAssets.Scripts.Inventory_Items;
using UnityEngine;

[Serializable]
public class Item : MonoBehaviour
{
    private BasicMovement player;

    [SerializeField]
    public Sprite InventoryImage;

    [SerializeField]
    private ItemType type;

    [SerializeField]
    private int itemHash; //must be equal to index in WorldManager.GiantItemList

    [SerializeField]
    private int maxStack;

    [SerializeField]
    private int amount = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        player = collision.gameObject.GetComponent<BasicMovement>();
        if (player != null)
        {
            player.IncludePickable(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        player = collision.gameObject.GetComponent<BasicMovement>();
        if (player != null)
        {
            player.ExcludePickable(this);
        }
    }

    public int Hash => itemHash;
    public ItemType itemType => type;
    public int MaxStack => maxStack;

    public int Amount
    {
        get => amount;
        set => amount = value;
    }


////abandon hope, ye who enters here

    public void ActWithChosenItem()
    {
        //todo: item must have an Action method, which later controls, how it is used
        /*if (items[chosenSlot])
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
        }*/
    }

    public void StopActing()
    {
        //Destroy(placedObject);
        //laddersMidPlacement = 0;
        //placing = false;
    }
    public void SwingPickaxe()
    {
    }
    //public void Shoot()
    //{
    //call GameObject.Instantiate from WorldManager
    //GameObject bullet = GameObject.Instantiate(itemValues.GetProjectile(atkType), projectilePosition, Quaternion.identity, GameObject.Find("Projectiles").transform);
    //bullet.GetComponent<Projectile>().ignore = GameObject.Find(masterName);
    //bullet.transform.right = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x - projectilePosition1.x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y - projectilePosition1.y);
    //bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.right * projectileVelocity;
    //}

}
