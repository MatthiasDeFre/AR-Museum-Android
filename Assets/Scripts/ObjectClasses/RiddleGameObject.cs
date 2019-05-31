using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bachelorproef.ObjectClasses {
public abstract class RiddleGameObject<T> : MonoBehaviour, IRiddleGameObject where T: Riddle
{
    public Painting Painting { get; set; }
    public T Riddle { get; set; }
    public MessageController MessageController { get; set; }
    protected void CheckIfComplete()
        {
            if (Riddle.Completed)
            {
                Debug.Log("RIDDLE UNLOCKED");
                // MessageController.showMessageType(MeType.Unlock, 'Paintings riddle.paintings.Names? unlocked!'
                // MessageController has a Queue / Set to make sure the same message only gets shown once
                // No longer needed :(
                // No need for paintingGameObject.ShowInfo => if in update will solve it
                Riddle.Paintings.ForEach(p => p.CheckIfUnlocked());
            }
        }
        
    public virtual void Init()
    {

    }
}
}