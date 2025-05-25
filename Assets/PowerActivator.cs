using UnityEngine;

public class PowerActivator : Collectible
{
    private DimensionSwitcher dimensionSwitcher;

    protected override void Collect()
    {
        if (dimensionSwitcher == null)
        {
            dimensionSwitcher = FindObjectOfType<DimensionSwitcher>();
        }

        if (dimensionSwitcher != null)
        {
            dimensionSwitcher.ActivatePower();
            Debug.Log("[PowerActivator] Power has been restored to the dimension switcher.");
        }
        else
        {
            Debug.LogWarning("[PowerActivator] DimensionSwitcher not found!");
        }

        base.Collect();
    }
}
