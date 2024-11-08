using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placeable : Item
{
    //todo: move all this shit into Placeable:Item

    //int amountOfLadders = 0;
    //int laddersMidPlacement = 0;
    //int ladderProtrudedness = 7;
    int maxProtrudeSteps = 7;

    public void UpdatePlacingObject()
    {
        /*if (placing)
        {
            CanPlaceLadder();
            Vector2 center = (Vector2)gameObject.transform.position + gameObject.GetComponent<CapsuleCollider2D>().offset;
            placedObject.transform.position = GlobalFuncs.GetMousePositionInRange(pickingDistance, center);
        }*/
    }

    public bool CanPlaceLadder()
    {
        /*Vector2 colliderPosition = placedObject.transform.GetChild(0).position;
        Vector2 colliderPosition2 = placedObject.transform.GetChild(placedObject.transform.childCount - 1).position;
        Vector2 colliderSize = placedObject.transform.GetChild(0).gameObject.GetComponent<BoxCollider2D>().size;

        Vector2 size = new Vector2(Mathf.Min(colliderSize.x, colliderSize.y), 0.01f);
        Vector2 pos1 = new Vector2(colliderPosition2.x + (colliderSize.y / 2 + 1.5f * size.y) * placedObject.transform.up.x, colliderPosition2.y + (colliderSize.y / 2 + 1.5f * size.y) * placedObject.transform.up.y);
        Vector2 pos2 = new Vector2(colliderPosition.x + (-colliderSize.y / 2 + 1.5f * size.y) * placedObject.transform.up.x, colliderPosition.y + (-colliderSize.y / 2 + 1.5f * size.y) * placedObject.transform.up.y);
        colliderSize = new Vector2(Mathf.Abs(colliderPosition2.x + colliderSize.x - colliderPosition.x), Mathf.Abs(colliderPosition2.y + colliderSize.y - colliderPosition.y));
        colliderSize = new Vector2(colliderSize.x * Mathf.Abs(placedObject.transform.up.y) + colliderSize.y * Mathf.Abs(placedObject.transform.up.x), colliderSize.y * Mathf.Abs(placedObject.transform.up.y) + colliderSize.x * Mathf.Abs(placedObject.transform.up.x));
        size = new Vector2(size.x * Mathf.Abs(placedObject.transform.up.y) + 0.01f, size.x * Mathf.Abs(placedObject.transform.up.x) + 0.01f);
        colliderPosition = new Vector2(colliderPosition.x+Mathf.Abs(colliderPosition.x-colliderPosition2.x)/2, colliderPosition.y + Mathf.Abs(colliderPosition.y - colliderPosition2.y) / 2);
        //RaycastHit2D[] rayCastUp = Physics2D.BoxCastAll(pos1, size, 0.0f, placedObject.transform.up, 0.05f, landLayer + platformLayer);
        //RaycastHit2D[] rayCastDown = Physics2D.BoxCastAll(pos2, size, 0.0f, placedObject.transform.up * -1.0f, 0.05f, landLayer + platformLayer);
        //RaycastHit2D[] rayCastMiddle = Physics2D.BoxCastAll(colliderPosition, colliderSize, 0.0f, placedObject.transform.up, 0.0f, landLayer + platformLayer);
        Debug.Log("1"+pos1.ToString());
        Debug.Log("2"+pos2.ToString());
        /*for (int i = 0; i < rayCastMiddle.Length; i++)
        {
            if (!rayCastMiddle[i].collider.gameObject.name.Contains("Ladder"))
            {
                SetColorAllLadders(Color.red);
                return false;
            }
        }
        if (rayCastUp.Length > 0)
        {
            SetColorAllLadders(Color.green);
            return true;
        }
        if (rayCastDown.Length > 0)
        {
            SetColorAllLadders(Color.green);
            return true;
        }
        SetColorAllLadders(Color.white);
        return true;*/ return false;
    }

    public void PlaceLadder()
    {
        /*if (!placing)
        {
            placedObject = Instantiate((GameObject)Resources.Load("Prefabs\\MultiLadder"), GetMousePositionInRange(pickingDistance), Quaternion.identity);
            placedObject.GetComponent<BoxCollider2D>().enabled = false;
            DisableLadderCollidersAndTransp(placedObject.transform.GetChild(0).gameObject);
            laddersMidPlacement = 1;
            placing = true;
        }
        else
        {
            if (CanPlaceLadder())
            {
                EnableAllLadderCollidersAndTransp();
                ladderProtrudedness = 7;
                amountOfLadders -= laddersMidPlacement;
                if (amountOfLadders == 0)
                {
                    items[2] = false;
                }
                placing = false;
            }
        }*/
    }
    void ProtrudeLadder()
    {
        /*if (ladderProtrudedness == maxProtrudeSteps)
        {
            if (amountOfLadders > laddersMidPlacement)
            {
                GameObject newLadder = Instantiate((GameObject)Resources.Load("Prefabs\\Ladder"), placedObject.transform.GetChild(placedObject.transform.childCount-1).position, placedObject.transform.rotation, placedObject.transform);
                DisableLadderCollidersAndTransp(newLadder);
                laddersMidPlacement++;
                ladderProtrudedness=1;
                ProtrudeOneStep(newLadder);
            }
        }
        else
        {
            ProtrudeOneStep(placedObject.transform.GetChild(placedObject.transform.childCount-1).gameObject);
            ladderProtrudedness++;
        }*/
    }
    void ProtrudeOneStep(GameObject lad)
    {
        lad.transform.position = new Vector2(lad.transform.position.x+(lad.GetComponent<BoxCollider2D>().size.y*lad.transform.up.x) / maxProtrudeSteps, lad.transform.position.y + (lad.GetComponent<BoxCollider2D>().size.y * lad.transform.up.y)/maxProtrudeSteps);
    }
    void RetrudeLadder()
    {
        /*if (laddersMidPlacement > 1)
        {
            if (ladderProtrudedness == 1)
            {
                Destroy(placedObject.transform.GetChild(placedObject.transform.childCount - 1).gameObject);
                laddersMidPlacement--;
                ladderProtrudedness = maxProtrudeSteps;
            }
            else
            {
                RetrudeOneStep(placedObject.transform.GetChild(placedObject.transform.childCount - 1).gameObject);
                ladderProtrudedness--;
            }
        }*/
    }
    void RetrudeOneStep(GameObject lad)
    {
        lad.transform.position = new Vector2(lad.transform.position.x - (lad.GetComponent<BoxCollider2D>().size.y * lad.transform.up.x) / maxProtrudeSteps, lad.transform.position.y - (lad.GetComponent<BoxCollider2D>().size.y * lad.transform.up.y) / maxProtrudeSteps);
    }

    void DisableLadderCollidersAndTransp(GameObject lad)
    {
        lad.GetComponent<BoxCollider2D>().enabled = false;
        lad.GetComponent<EdgeCollider2D>().enabled = false;
        GlobalFuncs.SetTransparency(lad, 0.5f);
    }
    void EnableAllLadderCollidersAndTransp()
    {
        /*placedObject.GetComponent<BoxCollider2D>().enabled = true;
        for (int i = 0; i < placedObject.transform.childCount; i++)
        {
            placedObject.transform.GetChild(i).gameObject.GetComponent<BoxCollider2D>().enabled = true;
            placedObject.transform.GetChild(i).gameObject.GetComponent<EdgeCollider2D>().enabled = true;
            GlobalFuncs.SetColor(placedObject.transform.GetChild(i).gameObject, Color.white);
            GlobalFuncs.SetTransparency(placedObject.transform.GetChild(i).gameObject, 1.0f);
        }*/
    }
    void SetColorAllLadders(Color col)
    {
        //for (int i = 0; i < placedObject.transform.childCount; i++)
        {
        //    GlobalFuncs.SetColor(placedObject.transform.GetChild(i).gameObject, col);
        }
    }
}
