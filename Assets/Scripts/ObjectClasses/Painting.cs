using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Painting 
{
   public bool Scanned { get; set; }
    public List<Riddle> RiddlesBeforeUnlock { get; set; }
    public List<Riddle> RiddlesToAttachToImage { get; set; }
    public string Url { get; set; }
    public Texture2D Image { get; set; }
    public int SortOrder { get; set; }
    public List<string> Info { get; set; }
    public int HeightPx { get; set; }
    public int WidthPx { get; set; }
    public string Name { get; set; }
    public string Date { get; set; }
    private bool unlocked;

    public bool Unlocked
    {
        get { return unlocked; }
    }
    public void CheckIfUnlocked()
    {
        var test = RiddlesBeforeUnlock.All(r => r.Completed);
        unlocked = test;
        Debug.Log("TET" + test);
    }

    public Painting(bool unlocked)
    {
        this.unlocked = unlocked;
    }
}
