using UnityEngine;

// Refactored Coin script to inherit from Collectible base class
public class Coin : Collectible
{
    protected override void Collect()
    {
        base.Collect(); // Call base class Collect()
    }
}
