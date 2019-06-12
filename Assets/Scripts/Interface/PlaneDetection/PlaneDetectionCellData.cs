using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneDetectionCellData : CellDataBase
{
    public int Index { get; set; }
    public string Id { get; set; }
    public Sprite Sprite { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public PlaneDetectionItemType Type { get; set; }
}
