using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using EnhancedUI.EnhancedScroller;
using TMPro;
using Doozy.Engine.UI;

namespace Bachelorproef.Interface
{
    /// <summary>
    /// This is the view for the rows
    /// </summary>
    public class InfoListRow : InfoListCellView
    {
        /// <summary>
        /// An internal reference to the row data. We could have just
        /// used the base CellView's class member _data, but that would
        /// require us to cast it each time a row data field is needed.
        /// By referencing the row data, we can save some time accessing
        /// the fields.
        /// </summary>
        private RowData _rowData;

        /// <summary>
        /// Links to the UI fields
        /// </summary>
        public TextMeshProUGUI InfoTest;
  
        /// <summary>
        /// Override of the base class's SetData function. This links the data
        /// and updates the UI
        /// </summary>
        /// <param name="data"></param>
        public override void SetData(InfoListData data, bool calculateLayout, string type)
        {
            // call the base SetData to link to the underlying _data
            base.SetData(data, calculateLayout, "ROW");

            // cast the data as rowData and store the reference
            _rowData = data as RowData;
            Debug.Log("PREF" + InfoTest.preferredHeight);
            // update the UI with the data fields
            InfoTest.text = _rowData.Data;
                // force update the canvas so that it can calculate the size needed for the text immediately
                Canvas.ForceUpdateCanvases();
                Debug.Log("CAL LAYOUT " + textRectTransform.rect.height);
            // set the data's cell size and add in some padding so the the text isn't up against the border of the cell
            data.CellSize = InfoTest.preferredHeight + textBuffer.top + textBuffer.bottom;

        }
    }
}