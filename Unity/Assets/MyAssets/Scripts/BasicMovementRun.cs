using UnityEngine;
public partial class BasicMovement: MonoBehaviour
{
    public float runMultiplier = 2.0f;
    public bool running;

    public void Run()
    {
        running = true;
    }

    public void UnRun()
    {
        running = false;
    }
}
