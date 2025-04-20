using UnityEngine;

// We extend our base Collectible class
public class PowerObstructor : Collectible
{
    public float disableDuration = 5f; // Time for which switching is disabled
    protected override void Collect()
    {
        base.Collect();
        Debug.Log("PO Collected!");
    }
}
