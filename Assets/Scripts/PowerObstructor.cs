using UnityEngine;
using System.Collections;

public class PowerObstructor : Collectible
{
    public float disableDuration = 5f; // Set from Inspector to as much seconds as you want
    private DimensionShift dimensionShift; 

    protected override void Collect()
    {
        // Find and disable dimension shifting mechanism
        if (dimensionShift == null)
        {
            dimensionShift = FindObjectOfType<DimensionShift>();
        }

        if (dimensionShift != null)
        {
            Debug.Log("[PowerObstructor] Disabling Dimension Shift for " + disableDuration + " seconds.");
            dimensionShift.DisableShift(disableDuration);
        }
        else
        {
            Debug.LogWarning("[PowerObstructor] DimensionShift script not found!");
        }

        base.Collect();
    }
}
