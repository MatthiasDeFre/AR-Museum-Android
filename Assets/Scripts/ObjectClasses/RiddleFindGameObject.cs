using Bachelorproef.ObjectClasses;
using Doozy.Engine.UI;
using GoogleARCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Bachelorproef.ObjectClasses
{
public class RiddleFindGameObject : RiddleGameObject<RiddleFind>
{
    // Get anchor, anchor me to painting location + anchor
    // Add touch listener to riddle prefab
    public AugmentedImage AugmentedImage { get; set; }
        private string colName;
        public GameObject RiddleCube { get; set; }
        private RiddleFindPaintingWithLoc riddleFindPaintingWithLoc;
        void Start()
        {
            RiddleCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            RiddleCube.GetComponent<MeshRenderer>().enabled = false;
            RiddleCube.transform.parent = transform.parent;
         
            riddleFindPaintingWithLoc = Riddle.PaintingsWithLocation.Find(loc => loc.PaintingId == Painting.SortOrder);
            //Change collider name to be unique
            var col = RiddleCube.GetComponent<Collider>();
            colName = $"RiddleFind_{Riddle.Id}_{Painting.SortOrder}";
            col.name = colName;
        }

        public override void Init()
    {
        base.Init();
        // Add
        // PaintingGameObject.Id TODO
        var id = 1;

        float halfWidth = AugmentedImage.ExtentX / 2;
        float halfHeight = AugmentedImage.ExtentZ / 2;
        Debug.Log("LEFT " + (halfWidth * Vector3.left) + (halfHeight * Vector3.back));
        Debug.Log("RIGHT" + (halfWidth * Vector3.right) + (halfHeight * Vector3.back));
        Debug.Log("MINUS" + ((halfWidth * Vector3.right) + (halfHeight * Vector3.back) - (halfWidth * Vector3.left) - (halfHeight * Vector3.back)));
    }
     void Update()
        {
            if (AugmentedImage == null || AugmentedImage.TrackingState != TrackingState.Tracking)
            {
                RiddleCube.SetActive(false);
               
                return;
            }
            if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began))
            {
                Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit raycastHit;
                if (Physics.Raycast(raycast, out raycastHit))
                {
                    if (raycastHit.collider.name == colName)
                    {
                        riddleFindPaintingWithLoc.Found = true;
                        CheckIfComplete();
                        Debug.Log("RIP RIDDLECUBE :(");
                        UIView.ShowView("PaintingList");
                        Destroy(RiddleCube);
                        Destroy(this);
                        return;
                    }
                }
            }
            PositionObject();
        }
        void PositionObject()
        {
            float halfWidth = AugmentedImage.ExtentX / 2;
            float halfHeight = AugmentedImage.ExtentZ / 2;
            int x = Painting.WidthPx;
            int y = Painting.HeightPx;
            float xTransitionFactor = x / AugmentedImage.ExtentX;
            float yTransitionFactor = y / AugmentedImage.ExtentZ;
            float startX = riddleFindPaintingWithLoc.StartLoc.x;
            float startY = riddleFindPaintingWithLoc.StartLoc.y;
            float transitionedStartX = startX / xTransitionFactor;
            float transitionedStartY = startY / yTransitionFactor;

            float endX = riddleFindPaintingWithLoc.EndLoc.x;
            float endY = riddleFindPaintingWithLoc.EndLoc.y;
            float transitionedEndX = endX / xTransitionFactor;
            float transitionedEndY = endY / yTransitionFactor;

            Vector3 transitionedStart = new Vector3(transitionedStartX, 0, transitionedStartY);
            Vector3 transitionedEnd = new Vector3(transitionedEndX, 0, transitionedEndY);
        
            RiddleCube.transform.localRotation = transform.localRotation;
            RiddleCube.transform.localScale = ((halfWidth * Vector3.left) + (halfHeight * Vector3.back) + transitionedStart) - ((halfWidth * Vector3.left) + (halfHeight * Vector3.back) + transitionedEnd);
            RiddleCube.transform.localPosition = (halfWidth * Vector3.left) + (halfHeight * Vector3.back) + transitionedStart - RiddleCube.transform.localScale / 2;

            RiddleCube.SetActive(true);
        }
}
}
