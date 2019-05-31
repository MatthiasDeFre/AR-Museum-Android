using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Riddle 
{
    [JsonProperty(PropertyName = "$id")]
    public string Id { get; }
    public string Hint { get; }
    public bool ShowOverlay { get; }
    public abstract bool Completed { get; }
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
