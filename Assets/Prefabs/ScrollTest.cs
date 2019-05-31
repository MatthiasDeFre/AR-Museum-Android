using EnhancedUI;
using EnhancedUI.EnhancedScroller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollTest : MonoBehaviour, IEnhancedScrollerDelegate
{
    private Story story;

    public Story Story
    {
        get { return story; }
        set { story = value; }
    }

    /// <summary>
    /// The list of inventory data
    /// </summary>
    private SmallList<PaintingCellData> _data;

    /// <summary>
    /// The vertical inventory scroller
    /// </summary>
    public EnhancedScroller vScroller;

    /// <summary>
    /// The horizontal inventory scroller
    /// </summary>
    public EnhancedScroller hScroller;

    /// <summary>
    /// The cell view prefab for the vertical scroller
    /// </summary>
    public EnhancedScrollerCellView vCellViewPrefab;

    /// <summary>
    /// The cell view prefab for the horizontal scroller
    /// </summary>
    public EnhancedScrollerCellView hCellViewPrefab;

    /// <summary>
    /// The image that shows which item is selected
    /// </summary>
    public Image selectedImage;
    public Text selectedImageText;

    /// <summary>
    /// The base path to the resources folder where the inventory
    /// item sprites are located
    /// </summary>
    public string resourcePath;
    public void SetActiveGameObject(string name)
    {
        Debug.Log("HEY");
    }
    void Awake()
    {
        // turn on the mask and loop functionality for each scroller based
        // on the UI settings of this controller

     /*   var maskToggle = GameObject.Find("Mask Toggle").GetComponent<Toggle>();
        MaskToggle_OnValueChanged(maskToggle.isOn);

        var loopToggle = GameObject.Find("Loop Toggle").GetComponent<Toggle>();
        LoopToggle_OnValueChanged(loopToggle.isOn);
        */
        CellViewSelected(null);
    }

    void Start()
    {
        // set up the delegates for each scroller

        hScroller.Delegate = this;

        // reload the data
        Reload();
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
        _data = new SmallList<PaintingCellData>();

        // add inventory items to the list
        _data.Add(new PaintingCellData() { Name = "Sword"});
        _data.Add(new PaintingCellData() { Name = "Sword"});
        _data.Add(new PaintingCellData() { Name = "Sword" });

        // tell the scrollers to reload
        hScroller.ReloadData();
    }

    /// <summary>
    /// This function handles the cell view's button click event
    /// </summary>
    /// <param name="cellView">The cell view that had the button clicked</param>
    private void CellViewSelected(EnhancedScrollerCellView cellView)
    {
        if (cellView == null)
        {
            // nothing was selected
        
        }
        else
        {
            // get the selected data index of the cell view
            var selectedDataIndex = (cellView as PaintingCellView).DataIndex;

            // loop through each item in the data list and turn
            // on or off the selection state. This is done so that
            // any previous selection states are removed and new
            // ones are added.
            for (var i = 0; i < _data.Count; i++)
            {
                _data[i].Selected = (selectedDataIndex == i);
            }

        }
    }

    #region Controller UI Handlers

    /// <summary>
    /// This handles the toggle for the masks
    /// </summary>
    /// <param name="val">Is the mask on?</param>
    public void MaskToggle_OnValueChanged(bool val)
    {
        // set the mask component of each scroller
        hScroller.GetComponent<Mask>().enabled = val;
    }

    /// <summary>
    /// This handles the toggle fof the looping
    /// </summary>
    /// <param name="val">Is the looping on?</param>
    public void LoopToggle_OnValueChanged(bool val)
    {
       
        hScroller.Loop = val;
    }

    #endregion

    #region EnhancedScroller Callbacks

    /// <summary>
    /// This callback tells the scroller how many inventory items to expect
    /// </summary>
    /// <param name="scroller">The scroller requesting the number of cells</param>
    /// <returns>The number of cells</returns>
    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        return _data.Count;
    }

    /// <summary>
    /// This callback tells the scroller what size each cell is.
    /// </summary>
    /// <param name="scroller">The scroller requesting the cell size</param>
    /// <param name="dataIndex">The index of the data list</param>
    /// <returns>The size of the cell (Height for vertical scrollers, Width for Horizontal scrollers)</returns>
    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
            // return a static width for all horizontal scroller cells
            return 150f;
        
    }

    /// <summary>
    /// This callback gets the cell to be displayed by the scroller
    /// </summary>
    /// <param name="scroller">The scroller requesting the cell</param>
    /// <param name="dataIndex">The index of the data list</param>
    /// <param name="cellIndex">The cell index (This will be different from dataindex if looping is involved)</param>
    /// <returns>The cell to display</returns>
    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        // first get a cell from the scroller. The scroller will recycle if it can.
        // We want a vertical cell prefab for the vertical scroller and a horizontal
        // prefab for the horizontal scroller.
        PaintingCellView cellView = scroller.GetCellView(hCellViewPrefab) as PaintingCellView;

        // set the name of the cell. This just makes it easier to see in our
        // hierarchy what the cell is
        cellView.name = ("Horizontal") + " " + _data[dataIndex].Name;

        // set the selected callback to the CellViewSelected function of this controller. 
        // this will be fired when the cell's button is clicked
        cellView.selected = CellViewSelected;

        // set the data for the cell
        cellView.SetData(dataIndex, _data[dataIndex], (false));

        // return the cell view to the scroller
        return cellView;
    }

    #endregion
}

