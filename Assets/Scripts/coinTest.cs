using UnityEngine;

public class CoinTest : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) // T key to play sound 
        {
            Coin coin = FindAnyObjectByType<Coin>(); // Find any coin object
            if (coin != null)
            {
                coin.CollectCoin(); // Call the function to collect them
                Debug.Log("Triggered Coin Collection");
            }
            else
            {
                Debug.Log("No Coin object found!");
            }
        }
    }
}
