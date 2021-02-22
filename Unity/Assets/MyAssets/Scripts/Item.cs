using UnityEngine;

public class Item : MonoBehaviour
{
    public Sprite InventoryImage;
    public int number;
    void Start()
    {
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerControls>() != null)
        {
            collision.gameObject.GetComponent<PlayerControls>().IncludePickable(this.gameObject);
        }
        if (collision.gameObject.name == "Picker")
        {
            collision.gameObject.transform.parent.gameObject.GetComponent<PlayerControls>().IncludePickableCursor(this.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerControls>() != null)
        {
            collision.gameObject.GetComponent<PlayerControls>().ExcludePickable(this.gameObject);
        }
        if (collision.gameObject.name == "Picker")
        {
            collision.gameObject.transform.parent.gameObject.GetComponent<PlayerControls>().ExcludePickableCursor(this.gameObject);
        }
    }
}
