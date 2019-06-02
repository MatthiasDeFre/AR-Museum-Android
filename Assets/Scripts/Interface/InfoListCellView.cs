using UnityEngine;
using System.Collections;
using EnhancedUI.EnhancedScroller;
using TMPro;
namespace Bachelorproef.Interface
{
    /// <summary>
    /// This is the base class for the different cell types. We use a base class
    /// to make calling SetData easier in the demo script.
    /// </summary>
    public class InfoListCellView : EnhancedScrollerCellView
    {
        /// <summary>
        /// Internal reference to our base data class
        /// </summary>
        protected InfoListData _data;
        public RectTransform textRectTransform;

        /// <summary>
        /// The space around the text label so that we
        /// aren't up against the edges of the cell
        /// </summary>
        public RectOffset textBuffer;
        /// <summary>
        /// Sets the data for the cell view. Note that the base data class is passed in,
        /// but through polymorphism we will actually pass the inherited data classes
        /// </summary>
        /// <param name="data"></param>
        public virtual void SetData(InfoListData data, bool calculateLayout, string type)
        {
            Debug.Log("CALLCING " + type);
            _data = data;
          
        }
    }
}