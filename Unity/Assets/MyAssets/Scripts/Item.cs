using UnityEngine;

public class Item : MonoBehaviour
{
    public float projectileVelocity = 50.0f;
    public ItemCharacteristics itemValues = new ItemCharacteristics();
    public Sprite InventoryImage;
    public int number;

    void Start()
    {
        itemValues.SetBuffs(new Buff(1, 10), new Buff(1, 3), new Buff(1, 1));
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerControls>() != null)
        {
            collision.gameObject.GetComponent<PlayerControls>().IncludePickable(this.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerControls>() != null)
        {
            collision.gameObject.GetComponent<PlayerControls>().ExcludePickable(this.gameObject);
        }
    }
}
