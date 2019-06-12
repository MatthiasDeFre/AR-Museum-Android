using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintingPrefab : MonoBehaviour
{

    public GameObject PlaneForPainting;
    public GameObject LeftFrame;
    public GameObject RightFrame;
    public GameObject UpperFrame;
    public GameObject LowerFrame;
    // 1 px is equal to this =>
    // Image of 1280 by 820 would be 13 by 8.328125
    // This will result in X*****X
    //                     *     *
    //                     x*****x
    // To fill in the corners add 0.5 to both for crossed corners only add 0.5 to one for single corners 
    private float pixelToScaleFactor = 0.01F;
    private float woodThickness = 0.25F;
    private float sizeFactor = 1250F;
   
    public void SetPaintingSprite(Sprite painting)
    {
        transform.localScale = new Vector3(1, 1, 1);
        var spriteRend = PlaneForPainting.GetComponent<SpriteRenderer>();
        spriteRend.sprite = painting;

        float width = painting.texture.width;
        float height = painting.texture.height;

        float normalizedWidth;
        float normalizedHeight;
        float normalizeFactor;
        if (width > height)
        {
            normalizeFactor = width / sizeFactor;
            normalizedWidth = sizeFactor;
            normalizedHeight = height / normalizeFactor;
        }
        else
        {
            normalizeFactor = height / sizeFactor;
            normalizedHeight = sizeFactor;
            normalizedWidth = width / normalizeFactor;
        }

        // Scale objects
        PlaneForPainting.transform.localScale = new Vector3(1,1,1) / normalizeFactor;
        LeftFrame.transform.localScale = new Vector3(woodThickness, normalizedHeight * pixelToScaleFactor + woodThickness, woodThickness);
        RightFrame.transform.localScale = new Vector3(woodThickness, normalizedHeight * pixelToScaleFactor + woodThickness, woodThickness);
        UpperFrame.transform.localScale = new Vector3(woodThickness, normalizedWidth * pixelToScaleFactor + woodThickness * 2, woodThickness);
        LowerFrame.transform.localScale = new Vector3(woodThickness, normalizedWidth * pixelToScaleFactor + woodThickness * 2, woodThickness);

        // Position objects
        Vector3 minBounds = spriteRend.bounds.min;
        Vector3 maxBounds = spriteRend.bounds.max;
        Debug.Log("MIN " + minBounds);
        Debug.Log("MAX " + maxBounds);
        float distanceY = (maxBounds.y - minBounds.y) / 2;
        float distanceZ = (maxBounds.z - minBounds.z) / 2;
        // wood / 2 => pivot is in the middle so only need to move it half
        // Left and Lower = Min bounds 
        LeftFrame.transform.localPosition = new Vector3(0, 0, -distanceZ - woodThickness / 2);
        LowerFrame.transform.localPosition = new Vector3(0, -distanceY - woodThickness / 2);

        // Right and Upper
        RightFrame.transform.localPosition = new Vector3(0, 0, distanceZ + woodThickness / 2);
        UpperFrame.transform.localPosition = new Vector3(0, distanceY + woodThickness / 2);
        transform.localScale = new Vector3(0.1F, 0.1F, 0.1F);
    }
}
