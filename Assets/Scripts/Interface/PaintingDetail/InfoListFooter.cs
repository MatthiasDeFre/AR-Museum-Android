using UnityEngine;
using System.Collections;
using EnhancedUI.EnhancedScroller;

namespace Bachelorproef.Interface
{
    /// <summary>
    /// This is the footer class. We could have just used the base CellView
    /// class for the footer cells, but it is created here for completeness of 
    /// the example.
    /// </summary>
    public class InfoListFooter : InfoListCellView
    {
        public override void SetData(InfoListData data, bool calculateLayout, string type)
        {
            base.SetData(data, false, "FOOTER");
        }
    }
}