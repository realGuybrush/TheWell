using UnityEngine;

public class ClimbableObject : MonoBehaviour {
    //todo: add y movement speed modifier here?

    private void OnTriggerEnter2D(Collider2D collision) {
        collision.gameObject.GetComponent<BasicMovement>()?.SteppedOnClimbable();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collision.gameObject.GetComponent<BasicMovement>()?.SteppedOffClimbable();
    }
}
