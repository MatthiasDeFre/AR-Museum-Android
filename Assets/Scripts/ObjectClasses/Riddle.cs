using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Riddle 
{
    
    public string Id { get; }
    public string Hint { get; set; }
    public bool ShowOverlay { get; }
    public abstract bool Completed { get; }
    [JsonIgnore]
    public List<Painting> Paintings { get; set; }
    public List<int> PaintingIds { get; set; }

    protected bool completed = false;
    public virtual void CheckIfCompleted()
    {

    }

    public Riddle()
    {
        Paintings = new List<Painting>();
    }

}
