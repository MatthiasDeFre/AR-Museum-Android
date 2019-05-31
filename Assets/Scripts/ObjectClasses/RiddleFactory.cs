using Bachelorproef;
using Bachelorproef.ObjectClasses;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiddleFactory : ScriptableObject
{
    public static void CreateRiddleGameObject(Riddle riddle, Transform parent, PaintingGameObject pDb)
    {

        switch(riddle)
        {
            case RiddleFind riddleFind:
                {
                    Debug.Log("FIND");
                    var temp_riddleGameObject = (RiddleFindGameObject)pDb.gameObject.AddComponent(typeof(RiddleFindGameObject));
                    temp_riddleGameObject.Riddle = riddleFind;
                    temp_riddleGameObject.AugmentedImage = pDb.AugmentedImage;
                    temp_riddleGameObject.Painting = pDb.Painting;
                    temp_riddleGameObject.transform.parent = parent;
                  //  temp_riddleGameObject.Init();
                    break;
                }      
            case RiddleScan riddleScan:
                {
                    Debug.Log("FOUND");
                    var temp_riddleGameObject = (RiddleScanGameObject)pDb.gameObject.AddComponent(typeof(RiddleScanGameObject));
                    temp_riddleGameObject.Riddle = riddleScan;
                    Debug.Log("HASH " + pDb.Painting.GetHashCode());
                    temp_riddleGameObject.Painting = pDb.Painting;
                    Debug.Log("HASH2 " + temp_riddleGameObject.Painting.GetHashCode());
                    temp_riddleGameObject.transform.parent = parent;
                 //   temp_riddleGameObject.Init();
                    break;
                }
        }

    }
}
