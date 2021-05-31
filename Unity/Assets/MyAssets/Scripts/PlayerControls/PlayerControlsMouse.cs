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
    public Vector2 GetMousePositionInRange(float range, Vector2 center)
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float distance = GlobalFuncs.Distance(center, pos);
        if (distance < range)
            return pos;
        else
        {
            Vector2 newPos = new Vector2(center.x - (center.x - pos.x) / (distance / range), center.y - (center.y - pos.y) / (distance / range));
            return newPos;
        }
    }
    public float GetMouseAngleRespToCenter(Vector2 center)
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float distance = GlobalFuncs.Distance(center, pos);
        float lowerPart = (pos.y - center.y) < 0.0f?1.0f:0.0f;
        float arccos = Mathf.Rad2Deg* Mathf.Acos((pos.x - center.x) / distance);
        return (pos.y - center.y) < 0.0f ? 180.0f + (180.0f - arccos):arccos;
    }
    public float GetMouseAngleWithFlip(Vector2 center)
    {
        float returnAngle = GetMouseAngleRespToCenter(center);
        if (!facingRight)
            returnAngle = 180.0f - returnAngle;
        if (returnAngle < 0)
            returnAngle += 360.0f;
        return returnAngle;
    }
    public void CheckCursorAngle()
    {
        anim.SetVar("Aim", GetMouseAngleWithFlip(GetCenterOfShootPartRotation()));
    }
}
