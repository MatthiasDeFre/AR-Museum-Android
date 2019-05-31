using Bachelorproef.ObjectClasses;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiddleScanGameObject : RiddleGameObject<RiddleScan>
{
 
    void Update()
    {
        Debug.Log("SCANNING");
        if (Painting.Scanned)
        {
            Debug.Log("SCANNED " + Painting.SortOrder + " " + Painting.GetHashCode());
            CheckIfComplete();
            Destroy(this);
        }
    }
}
