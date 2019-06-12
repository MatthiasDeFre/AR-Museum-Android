using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Newtonsoft.Json;

public class Painting 
{
   public bool Scanned { get; set; }
  
    public List<Riddle> RiddlesBeforeUnlock { get; set; }
   
    public List<Riddle> RiddlesToAttachToImage { get; set; }
    public string Url { get; set; }
    [JsonIgnore]
    public Texture2D Image { get; set; }
    [JsonIgnore]
    public Sprite Sprite { get; set; }
    public int SortOrder { get; set; }
    public List<string> Info { get; set; }
    public int HeightPx { get; set; }
    public int WidthPx { get; set; }
    public string Name { get; set; }
    public string Date { get; set; }
    public string PathName { get; set; }
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
