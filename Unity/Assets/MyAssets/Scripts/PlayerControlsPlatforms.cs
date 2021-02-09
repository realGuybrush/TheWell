using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerControls : BasicMovement
{
    bool fallingFromPlatform = false;

    public IEnumerator FallFromPlatform()
    {
        fallingFromPlatform = true;
        Physics2D.IgnoreLayerCollision(0, 11, true);
        yield return new WaitForSeconds(0.5f);
        Physics2D.IgnoreLayerCollision(0, 11, false);
        fallingFromPlatform = false;
    }
}
