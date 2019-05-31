using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public delegate void SelectedChangedDelegate(bool val);

public class PaintingCellData 
{
    public string Info;
    public string Name;
    public Texture2D tex;

        /// <summary>
        /// The delegate to call if the data's selection state
        /// has changed. This will update any views that are hooked
        /// to the data so that they show the proper selection state UI.
        /// </summary>
        public SelectedChangedDelegate selectedChanged;

        /// <summary>
        /// The selection state
        /// </summary>
        private bool _selected;
        public bool Selected
        {
            get { return _selected; }
            set
            {
                // if the value has changed
                if (_selected != value)
                {
                    // update the state and call the selection handler if it exists
                    _selected = value;
                    if (selectedChanged != null) selectedChanged(_selected);
                }
            }
        }
    
}
