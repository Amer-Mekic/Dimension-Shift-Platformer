using UnityEngine;

public class DimensionSwitcher : MonoBehaviour
{
    public GameObject[] dimension1Objects;
    public GameObject[] dimension2Objects;

    private bool isDimension1Active = true;
    private bool isPowerActive = false;

    void Start()
    {
        SetAllInactive(); // Initially everything is off
    }

    void Update()
    {
        if (isPowerActive && Input.GetKeyDown(KeyCode.Z))
        {
            isDimension1Active = !isDimension1Active;
            SetDimensionState(isDimension1Active);
        }
    }

    void SetAllInactive()
    {
        foreach (GameObject obj in dimension1Objects)
        {
            if (obj != null) obj.SetActive(false);
        }

        foreach (GameObject obj in dimension2Objects)
        {
            if (obj != null) obj.SetActive(false);
        }
    }

    void SetDimensionState(bool dim1Active)
    {
        foreach (GameObject obj in dimension1Objects)
        {
            if (obj != null) obj.SetActive(dim1Active);
        }

        foreach (GameObject obj in dimension2Objects)
        {
            if (obj != null) obj.SetActive(!dim1Active);
        }
    }

    public void ActivatePower()
    {
        isPowerActive = true;
        SetDimensionState(isDimension1Active); // Start with dimension 1
        Debug.Log("[DimensionSwitcher] Power activated!");
    }
}
