using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace Bachelorproef.ObjectClasses
{
public class RiddleFind : Riddle
{
    public List<RiddleFindPaintingWithLoc> PaintingsWithLocation { get; set; }

        public override bool Completed => PaintingsWithLocation.All(p => p.Found);

        public override void CheckIfCompleted()
    {
        throw new System.NotImplementedException();
    }
        public RiddleFind()
        {

        }
}

}