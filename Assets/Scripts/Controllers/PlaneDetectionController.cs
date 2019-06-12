using Doozy.Engine.UI;
using EnhancedUI;
using EnhancedUI.EnhancedScroller;
using GoogleARCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;
public class PlaneDetectionController : MonoBehaviour, IEnhancedScrollerDelegate
{
    public List<Painting> Paintings { get; set; }
    /// <summary>
    /// The list of inventory data
    /// </summary>
    private SmallList<PlaneDetectionCellData> _data;
    public EnhancedScrollerCellView CellViewPrefab;
    public GameObject WallPrefab;
    public PaintingPrefab PaintingPrefab;
    public PaintingInfo InfoPrefab;
    public Sprite WallSprite;
    private Dictionary<string, GameObject> placedObjects = new Dictionary<string, GameObject>(); 
    private int activeIndex = -1;
    private bool isPlaceMode = true;
    private UIView view;
    /// <summary>
    /// The first-person camera being used to render the passthrough camera image (i.e. AR
    /// background).
    /// </summary>
    public Camera FirstPersonCamera;
    public EnhancedScroller Scroller;
    /// <summary>
    /// The rotation in degrees need to apply to model when the Andy model is placed.
    /// </summary>
    private const float k_ModelRotation = 180.0f;

    /// <summary>
    /// True if the app is in the process of quitting due to an ARCore connection error,
    /// otherwise false.
    /// </summary>
    private bool m_IsQuitting = false;

    // Start is called before the first frame update


    void Start()
    {
      
        // set up the delegates for each scroller
        // Layout fix
        var scrollRect = Scroller.GetComponent<ScrollRect>();
        scrollRect.content.offsetMax = new Vector2(scrollRect.content.offsetMax.x, 0);
        scrollRect.content.offsetMin = new Vector2(scrollRect.content.offsetMin.x, 0);
        Scroller.Delegate = this;
        m_Raycaster = GetComponent<GraphicRaycaster>();
    }

    /// <summary>
    /// This function sets up our inventory data and tells the scrollers to reload
    /// </summary>
    private void Reload()
    {
        // if the data existed previously, loop through
        // and remove the selection change handlers before
        // clearing out the data.

        // set up a new inventory list
        _data = new SmallList<PlaneDetectionCellData>();
        var filteredPaintings = Paintings.Where(p => p.Unlocked).ToList();
        var indexCounter = 0;
        _data.Add(new PlaneDetectionCellData() { Type=PlaneDetectionItemType.Wall ,Index= indexCounter++,  Id= "Wall-0", Description= "Plaats dit om je virtueel museum te beginnen", Sprite=WallSprite, Title="Virtueel Museum"});
        filteredPaintings.ForEach(p =>
        {
            _data.Add(new PlaneDetectionCellData() { Type=PlaneDetectionItemType.Painting, Index = indexCounter++, Id = $"Painting-{p.SortOrder}", Title = "Schilderij", Description = p.Name, Sprite = p.Sprite });
            int infoCounter = 0;
            p.Info.ForEach(info =>
            {
                _data.Add(new PlaneDetectionCellData() {Type=PlaneDetectionItemType.Info, Index=indexCounter++, Id = $"Info-{p.SortOrder}-{infoCounter++}", Title = $"Info {infoCounter} van {p.Name}", Description = info, Sprite= p.Sprite });
            });

        });
        // add inventory items to the list
     

        // tell the scrollers to reload
        Scroller.ReloadData();
    }
    // Update is called once per frame
    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    public EventSystem m_EventSystem;
    void Update()
    {
      
        // activeIndex == -1 => Nothing has been selected yet
        // If the player has not touched the screen, we are done with this update.
        Touch touch;
        if (activeIndex == -1 || Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
        {
            return;
        }
        Debug.Log("TOUCH");
        //Set up the new Pointer Event
        m_PointerEventData = new PointerEventData(m_EventSystem);
        //Set the Pointer Event Position to that of the mouse position
        m_PointerEventData.position = touch.position;

        //Create a list of Raycast Results
        List<RaycastResult> results = new List<RaycastResult>();

        //Raycast using the Graphics Raycaster and mouse click position
        m_Raycaster.Raycast(m_PointerEventData, results);

        //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
        bool uiFound = results.Any(r => r.gameObject.tag != "UI-Ignore");
        Debug.Log("UIFOUND " + uiFound);
        if (uiFound) return;

        // HIT PLANE OR GAMEOBJECT SO SELECT PREFAB TO PLACE => DO NOT INSTANIATE WE DONT KNOW THE LOCATION YET
        GameObject prefabToPlace = null;
        PlaneDetectionCellData cellData = _data[activeIndex];
        switch (cellData.Type)
        {
            case PlaneDetectionItemType.Wall:
                {
                    prefabToPlace = WallPrefab;
                    break;
                }
            case PlaneDetectionItemType.Painting:
                {
                    PaintingPrefab.SetPaintingSprite(cellData.Sprite);
                    PaintingPrefab.transform.localScale = new Vector3(0.125F, 0.125F, 0.125F);
                    prefabToPlace = PaintingPrefab.gameObject;
                    break;
                }
            case PlaneDetectionItemType.Info:
                {
                    InfoPrefab.UpdateText(cellData.Description);
                    prefabToPlace = InfoPrefab.gameObject;
                    break;
                }

        }

        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 9;
        Ray ray = Camera.main.ScreenPointToRay(touch.position);
        RaycastHit objhit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(ray, out objhit, 100F))
        {   
            Debug.Log("Did Hit " + objhit.transform.gameObject.tag);
            if(isPlaceMode)
            {
            var objectToPlace = Instantiate(prefabToPlace, objhit.point, objhit.transform.rotation);
            Debug.Log("ROT 1 " + objhit.transform.rotation);
            Debug.Log("ROT 2 " + objectToPlace.transform.rotation);
            objectToPlace.transform.parent = objhit.transform;
            var inversePoint = objhit.transform.InverseTransformPoint(objhit.point);
            var movement = inversePoint.x < 0 ? new Vector3(-0.1F, 0, 0) : new Vector3(0.1F, 0, 0);
            objectToPlace.transform.localPosition = inversePoint + movement;
            Debug.Log("POINT  " + objhit.transform.InverseTransformPoint(objhit.point));
            Debug.Log("POINT 2 " + objhit.transform.position);
            
            objectToPlace.transform.LookAt(objhit.transform);
            GameObject previousObject = null;
            if (placedObjects.TryGetValue(cellData.Id, out previousObject))
            {
                Destroy(previousObject);
                placedObjects.Remove(cellData.Id);
            }
        //    objectToPlace.transform.Rotate(0, prefabToPlace.transform.localRotation.y, 0, Space.Self);
            //objectToPlace.transform.LookAt(objhit.transform.gameObject.transform);
            placedObjects.Add(cellData.Id, objectToPlace);
            } else
            {
                Destroy(objhit.transform.parent.gameObject);
            }
            return;
        }
        if (!isPlaceMode) return;
        // Raycast against the location the player touched to search for planes.
        TrackableHit hit;
        TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon |
            TrackableHitFlags.FeaturePointWithSurfaceNormal;

        if (Frame.Raycast(touch.position.x, touch.position.y, raycastFilter, out hit))
        {
            Debug.Log("git");

            // Use hit pose and camera pose to check if hittest is from the
            // back of the plane, if it is, no need to create the anchor.
            if ((hit.Trackable is DetectedPlane) &&
                Vector3.Dot(FirstPersonCamera.transform.position - hit.Pose.position,
                    hit.Pose.rotation * Vector3.up) < 0)
            {
                Debug.Log("Hit at back of the current DetectedPlane");
            }
            else
            {
                Debug.Log("ANDY");
                // Choose the Andy model for the Trackable that got hit.
                // Objects can only be placed once => destroy previous object
                GameObject previousObject = null;
                if (placedObjects.TryGetValue(cellData.Id, out previousObject))
                {
                    Destroy(previousObject);
                    placedObjects.Remove(cellData.Id);
                }
                // Instantiate Andy model at the hit pose.
                var objectToPlace = Instantiate(prefabToPlace, hit.Pose.position, hit.Pose.rotation);
                placedObjects.Add(cellData.Id, objectToPlace);
                // Compensate for the hitPose rotation facing away from the raycast (i.e.
                // camera).
                //objectToPlace.transform.Rotate(0, prefabToPlace.transform.localRotation.y, 0, Space.Self);
                objectToPlace.SetActive(true);
                // Create an anchor to allow ARCore to track the hitpoint as understanding of
                // the physical world evolves.
                var anchor = hit.Trackable.CreateAnchor(hit.Pose);

                // Make Andy model a child of the anchor.
                objectToPlace.transform.parent = anchor.transform;
                Debug.Log(objectToPlace.transform.localScale);
                Debug.Log(objectToPlace.transform.localPosition);
                Debug.Log(objectToPlace.transform.localRotation);

                // objectToPlace.transform.localScale = new Vector3(0.25F, 0.25F, 0.25F);
            }
        }
    }

    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        // in this example, we just pass the number of our data elements
        return _data.Count;
    }

    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        // we will determine the cell height based on what kind of row it is
        return 870F;
    }

    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        PlaneDetectionCellView cellView = scroller.GetCellView(CellViewPrefab) as PlaneDetectionCellView;

        // set the name of the cell. This just makes it easier to see in our
        // hierarchy what the cell is
        cellView.name = ("Horizontal") + " " + _data[dataIndex].Id;

        // set the selected callback to the CellViewSelected function of this controller. 
        // this will be fired when the cell's button is clicked
        cellView.selected = CellViewSelected;

        // set the data for the cell
        cellView.SetData(dataIndex, _data[dataIndex], (false));

        // return the cell view to the scroller
        return cellView;
    }

    /// <summary>
    /// This function handles the cell view's button click event
    /// </summary>
    /// <param name="cellView">The cell view that had the button clicked</param>
    private void CellViewSelected(int index)
    {
     
        activeIndex = index;
        // TODO Create gameobject!
        //this.activeObject = activeObject;
    }
    public void Show()
    {
        if(view == null)
        {
            view = GetComponent<UIView>();
        }
        Reload();
        view.Show();
    }
    public void Hide()
    {
        if (view == null)
        {
            view = GetComponent<UIView>();
        }
        view.Hide();
    }
    public void SwitchPlaceMode(bool place)
    {
        Debug.Log("MODE " + place);
        isPlaceMode = place;
    }
    public void RemoveAll()
    {
        foreach(var keyValue in placedObjects)
        {
            Destroy(keyValue.Value);
            placedObjects.Remove(keyValue.Key);
        }
    }
}
