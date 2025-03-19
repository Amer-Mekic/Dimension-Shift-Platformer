using UnityEngine;

// Same refactoring as in Coin script, extend our custom Collectible class
public class Star : Collectible
{
    protected override void Collect()
    {
        base.Collect();
        Debug.Log("Star Collected!");
    }
}
