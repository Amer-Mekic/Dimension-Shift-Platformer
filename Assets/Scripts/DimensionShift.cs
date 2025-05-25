using UnityEngine;
using System.Collections.Generic;

public class DimensionShift : MonoBehaviour
{
    public string firstLayerName = "Dimension1";   // Layer name for first dimension
    public string secondLayerName = "Dimension2";  // Layer name for second dimension

    private List<GameObject> firstObjects = new List<GameObject>();
    private List<GameObject> secondObjects = new List<GameObject>();
    private bool isFirstActive = true;
    private bool isBlocked = false;
    private float blockEndTime = 0f;

    void Start()
    {
        int firstLayer = LayerMask.NameToLayer(firstLayerName);
        int secondLayer = LayerMask.NameToLayer(secondLayerName);

        if (firstLayer == -1 || secondLayer == -1)
        {
            Debug.LogError("[DimensionShift] One or both layer names are invalid.");
            return;
        }

        firstObjects.AddRange(FindObjectsInLayerIncludingInactive(firstLayer));
        secondObjects.AddRange(FindObjectsInLayerIncludingInactive(secondLayer));

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

    // Finds objects even if they are inactive, and filters them by layer
    GameObject[] FindObjectsInLayerIncludingInactive(int layer)
    {
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        List<GameObject> layerObjects = new List<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.layer == layer && obj.hideFlags == HideFlags.None)
            {
                layerObjects.Add(obj);
            }
        }

        return layerObjects.ToArray();
    }
}
