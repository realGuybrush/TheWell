using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerControls : BasicMovement
{
    public Vector2 GetMousePosition()
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return pos;
    }
    public Vector2 GetMousePositionInRange(float range)
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 center = (Vector2)this.gameObject.transform.position + this.gameObject.GetComponent<CapsuleCollider2D>().offset;
        float distance = GlobalFuncs.Distance(center, pos);
        if (distance < range)
            return pos;
        else
        {
            Vector2 newPos = new Vector2(center.x-(center.x - pos.x)/(distance/range), center.y - (center.y - pos.y) / (distance / range));
            return newPos;
        }
    }
}
