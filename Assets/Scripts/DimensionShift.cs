using UnityEngine;
using System.Collections.Generic;

public class DimensionShift : MonoBehaviour
{
    public string firstTag = "Dimension1";  // Tag for first dimension
    public string secondTag = "Dimension2"; // Tag for second dimension

    private List<GameObject> firstObjects = new List<GameObject>();
    private List<GameObject> secondObjects = new List<GameObject>();
    private bool isFirstActive = true;
    private bool isBlocked = false;
    private float blockEndTime = 0f;

    void Start()
    {
        // Populate lists even if objects are inactive
        firstObjects.AddRange(FindObjectsByTagIncludingInactive(firstTag));
        secondObjects.AddRange(FindObjectsByTagIncludingInactive(secondTag));

        // Initial state
        SetObjectsActive(firstObjects, true);
        SetObjectsActive(secondObjects, false); 
    }

    void Update()
    {
        if (isBlocked && Time.time >= blockEndTime)
        {
            isBlocked = false;
            Debug.Log("[DimensionShift] Shift UNBLOCKED");
        }

        if (Input.GetKeyDown(KeyCode.Z) && !isBlocked)
        {
            SwitchObjects();
        }
        else if (Input.GetKeyDown(KeyCode.Z) && isBlocked)
        {
            Debug.Log("[DimensionShift] Shift BLOCKED!");
        }
    }
    public void DisableShift(float duration)
    {
        isBlocked = true;
        blockEndTime = Time.time + duration;
        Debug.Log("[DimensionShift] Shift disabled for " + duration + "s");
    }

    void SwitchObjects()
    {
        isFirstActive = !isFirstActive;

        SetObjectsActive(firstObjects, isFirstActive);
        SetObjectsActive(secondObjects, !isFirstActive);
    }

    void SetObjectsActive(List<GameObject> objects, bool active)
    {
        foreach (GameObject obj in objects)
        {
            if (obj != null)
                obj.SetActive(active);
        }
    }

    // Custom method to find inactive objects too
    GameObject[] FindObjectsByTagIncludingInactive(string tag)
    {
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        List<GameObject> taggedObjects = new List<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.CompareTag(tag) && obj.hideFlags == HideFlags.None)
            {
                taggedObjects.Add(obj);
            }
        }
        return taggedObjects.ToArray();
    }
}