using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CellDataBase 
{
    public SelectedChangedDelegate selectedChanged;

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
                selectedChanged?.Invoke(_selected);
            }
        }
    }
}
