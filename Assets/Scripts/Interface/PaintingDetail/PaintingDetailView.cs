using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Doozy.Engine.UI;
using EnhancedUI.EnhancedScroller;
using EnhancedUI;
using Bachelorproef.Interface;
using System.Linq;
using System.Text.RegularExpressions;

public class PaintingDetailView : UIView, IEnhancedScrollerDelegate
{
    private SmallList<InfoListData> _data;
    private bool _calculateLayout;

    /// <summary>
    /// This is our scroller we will be a delegate for
    /// </summary>
    public EnhancedScroller Scroller;

    public Sprite LockedPainting;
    public InfoListCellView HeaderCellViewPrefab;
    public InfoListCellView RowCellViewPrefab;
    public InfoListCellView FooterCellViewPrefab;

    private Painting painting;

    public Painting Painting
    {
        get { return painting; }
        set { painting = value; UpdateUI(); }
    }

    private List<string> hiddenInfo;

    public Image Image;
    public TextMeshProUGUI Title;

    public override void Start()
    {
        base.Start();
        Scroller.Delegate = this;
    }

    private void UpdateUI()
    {
        // Need to set null to update aspect ratio ¯\_(ツ)_/¯
        Image.sprite = null;
   
     

        // Pattern to hide data when painting is not scanned / unlocked
        Regex pattern = new Regex("[a-zA-Z0-9]");
        // Hide Text if not scanned
        Image.sprite = painting.Unlocked ? painting.Sprite : LockedPainting;
        Debug.Log("LOG " + painting.Scanned);
        if (!painting.Scanned)
        {
            Title.text = pattern.Replace(painting.Name, "?");
            hiddenInfo = painting.Info.Select(i => pattern.Replace(i, "?")).ToList();
        } else
        {
            Title.text = painting.Name;
            hiddenInfo = painting.Info;    
        }
        var aspectRatio = Mathf.Max(1, Image.sprite.rect.width / Image.sprite.rect.height);
        var textMovementUpwards = Image.rectTransform.rect.height / aspectRatio;
        var pos = Image.transform.localPosition;
        // Only needs to move it / 2 because the pivot in in the middle of the image
        // /-----/
        // /--P--/ => P = Pivot
        // /----/
        Title.transform.localPosition = new Vector3(pos.x, pos.y - textMovementUpwards / 2, pos.z);
        var titlePos = Title.transform.localPosition;
        Scroller.transform.localPosition = new Vector3(titlePos.x, titlePos.y - Title.rectTransform.rect.height / 2, titlePos.z);
       
        LoadData();
    }

    public void Close()
    {
        Hide();
    }

    private void LoadData()
    {
        // create some data
        // note we are using different data class fields for the header, row, and footer rows. This works due to polymorphism.

        _data = new SmallList<InfoListData>();

        _data.Add(new HeaderData() { Category = "Informatie" });
        hiddenInfo.ForEach(info =>
        {
            _data.Add(new RowData() { Data = info });
        });
        _data.Add(new FooterData());
        List<string> riddleHint = Painting.RiddlesBeforeUnlock.Select(r => r.Hint).ToList();
        if(riddleHint.Count > 0)
        {
            _data.Add(new HeaderData() { Category = "Raadsel Hints" });
            riddleHint.ForEach(info =>
            {
                Debug.Log("Radd " + info);
                _data.Add(new RowData() { Data = info });
            });
            _data.Add(new FooterData());
        }

        // tell the scroller to reload now that we have the data
        ResizeScroller();
    }

    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        // in this example, we just pass the number of our data elements
        return _data.Count;
    }

    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        // we will determine the cell height based on what kind of row it is
        Debug.Log("NUMBER" + _data[dataIndex].CellSize);
        return _data[dataIndex].CellSize;
    }

    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        InfoListCellView cellView;

        // determin what cell view to get based on the type of the data row

        if (_data[dataIndex] is HeaderData)
        {
            // get a header cell prefab from the scroller, recycling old cells if possible
            cellView = scroller.GetCellView(HeaderCellViewPrefab) as InfoListHeader;
            Debug.Log("CEL" + HeaderCellViewPrefab);
            // optional for clarity: set the cell's name to something to indicate this is a header row
            cellView.name = "[Header] " + (_data[dataIndex] as HeaderData).Category;
        }
        else if (_data[dataIndex] is RowData)
        {
            // get a row cell prefab from the scroller, recycling old cells if possible
            cellView = scroller.GetCellView(RowCellViewPrefab) as InfoListRow;

            // optional for clarity: set the cell's name to something to indicate this is a row
            cellView.name = "[Row] " + (_data[dataIndex] as RowData).Data;
        }
        else
        {
            // get a footer cell prefab from the scroller, recycling old cells if possible
            cellView = scroller.GetCellView(FooterCellViewPrefab) as InfoListFooter;
            cellView.transform.localScale = new Vector3(2.3F, 1F, 1F);
            // optional for clarity: set the cell's name to something to indicate this is a footer row
            cellView.name = "[Footer]";
        }

        // set the cell view's data. We can do this because we declared a single SetData function
        // in the CellView base class, saving us from having to call this for each cell type
        cellView.SetData(_data[dataIndex], _calculateLayout, "NON");

        // return the cellView to the scroller
        return cellView;
    }

    private void ResizeScroller()
    {
        // capture the scroller dimensions so that we can reset them when we are done
        var rectTransform = Scroller.GetComponent<RectTransform>();
        var size = rectTransform.sizeDelta;

        // set the dimensions to the largest size possible to acommodate all the cells
        rectTransform.sizeDelta = new Vector2(size.x, float.MaxValue);

        // First Pass: reload the scroller so that it can populate the text UI elements in the cell view.
        // The content size fitter will determine how big the cells need to be on subsequent passes.
        _calculateLayout = true;
        Scroller.ReloadData();

        // reset the scroller size back to what it was originally
        rectTransform.sizeDelta = size;

        // Second Pass: reload the data once more with the newly set cell view sizes and scroller content size
        _calculateLayout = false;
        Scroller.ReloadData();
    }
}
