using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Story 
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public List<Riddle> Riddles { get; set; }
    public List<Painting> Paintings { get; set; }

   
    public Story()
    {

    }
}
