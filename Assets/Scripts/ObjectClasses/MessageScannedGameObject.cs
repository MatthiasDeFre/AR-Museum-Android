using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageScannedGameObject : MonoBehaviour
{
    public Text UpperText;
    public Text LowerText;
    private Animator animator;
    // Start is called before the first frame update
    public void Init()
    {
        animator = gameObject.GetComponent<Animator>();
    }
    public void HideObject()
    {
        gameObject.SetActive(false);
    }
    public void ShowObject()
    {
    }
    public void ShowMessage(string upperText, string lowerText)
    {
        UpperText.text = upperText;
        LowerText.text = lowerText;
        gameObject.SetActive(true);
        animator.SetTrigger("StartAnim");
    }
}
