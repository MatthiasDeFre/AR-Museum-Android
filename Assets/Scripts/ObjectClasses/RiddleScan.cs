using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Newtonsoft.Json;

namespace Bachelorproef.ObjectClasses
{
    public class RiddleScan : Riddle
    {
        [JsonIgnore]
        public List<Painting> PaintingsToScan = new List<Painting>();
        [JsonIgnore]
        public override bool Completed => PaintingsToScan.All(painting => {
            Debug.Log("PAINTING " + painting); 
            Debug.Log("SCAN " + painting.SortOrder + " " + painting.Scanned + " " + painting.GetHashCode());
            return painting.Scanned;
        });

        public RiddleScan() : base()
        {
            PaintingsToScan = new List<Painting>();
        }

        public override void CheckIfCompleted()
        {

        }
    }
}