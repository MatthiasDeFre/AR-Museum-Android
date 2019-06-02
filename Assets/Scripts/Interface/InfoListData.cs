using UnityEngine;
using System.Collections;

namespace Bachelorproef.Interface
{
    /// <summary>
    /// This set of classes store information about the different rows.
    /// The base data class has no members, but is useful for polymorphism.
    /// </summary>
    public class InfoListData
    {
        private float cellSize = 150F;
        public virtual float CellSize { get => cellSize; set { cellSize = value; } }
    }

    /// <summary>
    /// This is the data that the header rows will use. It only contains a category
    /// field.
    /// </summary>
    public class HeaderData : InfoListData
    {
        /// <summary>
        /// The category of header for the group
        /// </summary>
        public string Category;
    }

    /// <summary>
    /// This is the data that will store information about users within a group
    /// </summary>
    public class RowData : InfoListData
    {
        public string Data;
        
    }

    /// <summary>
    /// This is data for the footer. No actual fields are used in this class,
    /// but we set it up for completeness of the example.
    /// </summary>
    public class FooterData : InfoListData
    {
     public FooterData()
        {
            CellSize = 50F;
        }
    }
}