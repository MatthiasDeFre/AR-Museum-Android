using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageLockedGameObject : MonoBehaviour
{

    private Animator animator;
    // Start is called before the first frame update
    public void Init()
    {
        animator = gameObject.GetComponent<Animator>();
    }
 
    public void ShowObject()
    {
    }
    public void ShowMessage()
    {
        animator.SetTrigger("StartAnim");
    }
}
