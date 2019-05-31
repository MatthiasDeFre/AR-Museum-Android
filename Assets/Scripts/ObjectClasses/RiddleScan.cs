using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace Bachelorproef.ObjectClasses
{
    public class RiddleScan : Riddle
    {
        public List<Painting> PaintingsToScan;
        public override bool Completed => PaintingsToScan.All(painting => {
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