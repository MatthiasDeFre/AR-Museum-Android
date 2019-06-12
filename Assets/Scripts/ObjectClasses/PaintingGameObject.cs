using Bachelorproef.ObjectClasses;
using GoogleARCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintingGameObject : MonoBehaviour
{
    public Painting Painting { get; set; }
    public AugmentedImage AugmentedImage{ get; set; }
    //public GameObject Cube { get; set; }
    public PaintingInfo PaintingInfo;
    private List<PaintingInfo> infoObjects = new List<PaintingInfo>();
    public MessageController MessageController { get; set; }
    private bool first = true;
    // Start is called before the first frame update
    void Start()
    {
        /*Cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        var test = Resources.Load<GameObject>("Effect1");
        Cube.transform.parent = transform.parent;
        test.transform.parent = Cube.transform;
        test.transform.localPosition = Vector3.zero;*/
       // var psUpdater = test.GetComponent<PSMeshRendererUpdater>();
        InitPainting();
        try
        {
           // psUpdater.UpdateMeshEffect(Cube);
        }
        catch (Exception e)
        {
            Debug.Log("ERROR " + e.StackTrace);
            Debug.Log("ERROR " + e.Message);
        }

    }

    /// <summary>
    /// The Unity Update method.
    /// </summary>
    public void Update()
    {
        // Dont do anything if the ui has a open window and is not scanned
        // Position updates can happen if the painting is already scanned but not before that
        if (!Painting.Scanned && NavigationController.HasOpenWindow) return;

        if (!Painting.Unlocked || AugmentedImage == null || AugmentedImage.TrackingState != TrackingState.Tracking)
        {
          //  Cube.SetActive(false);
            infoObjects.ForEach(info => { 
                info.gameObject.SetActive(false);
            });
            if (AugmentedImage == null || AugmentedImage.TrackingState != TrackingState.Tracking)
            {
                return;
            } else if(first)
            {
                first = false;
                MessageController.ShowMessage(MessageType.Locked);
            }
            return;
        }
  
        if (AugmentedImage.TrackingMethod == AugmentedImageTrackingMethod.FullTracking && !Painting.Scanned)
        {
            MessageController.ShowMessage(MessageType.Scanned, new string[] { Painting.Date, Painting.Name });
            Painting.Scanned = true;
            StatController.IncrementStat(StatType.Scanned, 1);
            StatController.IncrementStat(StatType.Experience, 15);
        }

        float halfWidth = AugmentedImage.ExtentX / 2;
        float halfHeight = AugmentedImage.ExtentZ / 2;
        int x = 1000;
        int y = 1000;
        int locX = 500;
        int lockY = 500;
        float xTransitionFactor = x / AugmentedImage.ExtentX;
        float yTransitionFactor = y / AugmentedImage.ExtentZ;
        float transitionedLockX = locX / xTransitionFactor;
        float transitionedLockY = lockY / yTransitionFactor;
        Vector3 movement = new Vector3(transitionedLockX, 0, transitionedLockX);
      //  Cube.transform.localRotation = transform.localRotation;
       // Cube.transform.localScale = ((halfWidth * Vector3.right) + (halfHeight * Vector3.forward)) - ((halfWidth * Vector3.left) + (halfHeight * Vector3.back) + movement);
     //   Cube.transform.localPosition = (halfWidth * Vector3.left) + (halfHeight * Vector3.back) + movement + Cube.transform.localScale / 2;
        var distance = Vector3.Distance((halfWidth * Vector3.left) + (halfHeight * Vector3.back) + movement, (halfWidth * Vector3.right) + (halfHeight * Vector3.forward));
        float newSize = distance / 50;
        infoObjects.ForEach(info =>
        {
            info.transform.localPosition = ((halfWidth * 2.5F + 0.2F) * Vector3.left) + (AugmentedImage.ExtentZ * Vector3.back);
            info.transform.localRotation = transform.localRotation;
            // TODO I'll think about it
            //info.transform.LookAt(Camera.main.transform);
            info.transform.RotateAround(info.transform.position, info.transform.up, -90f);
            info.transform.RotateAround(info.transform.position, info.transform.forward, -90f);

            info.transform.localScale = new Vector3(0.2F, halfHeight / 2, halfWidth / 2);
            info.gameObject.SetActive(true);
        });
      //  Cube.transform.localScale = ((halfWidth * Vector3.right) + (halfHeight * Vector3.forward)) - ((halfWidth * Vector3.left) + (halfHeight * Vector3.back) + movement);

      //  Cube.SetActive(true);
    }

    public void InitPainting()
    {
        Painting.Info.ForEach(info =>
        {
           var infoObject = Instantiate(PaintingInfo, transform.parent);
           
           infoObject.UpdateText(info);
           infoObjects.Add(infoObject);
        });
        Painting.RiddlesToAttachToImage.ForEach(riddle => {
            Type rtype = riddle.GetType();
            RiddleFactory.CreateRiddleGameObject(riddle, transform.parent, this);
        });
    }
}
