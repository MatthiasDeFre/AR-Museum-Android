using Doozy.Engine.Progress;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaintingInfo : MonoBehaviour
{
    public Text TextObject;
    public GameObject LeftPanel;
    public GameObject RightPanel;
    public GameObject MiddlePanel;


    // Start is called before the first frame update
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateText(string text)
    {
        TextObject.text = text;
        var scaleYMultiplier = TextObject.preferredHeight / TextObject.fontSize;
        Debug.Log("SCALE TEST 1 " + scaleYMultiplier);
        Debug.Log("SCALE TEST 2 " + TextObject.preferredHeight);
        Debug.Log("SCALE TEST 3 " + TextObject.fontSize);
        LeftPanel.transform.localScale = new Vector3(LeftPanel.transform.localScale.x, scaleYMultiplier + 4, LeftPanel.transform.localScale.z);
        RightPanel.transform.localScale = new Vector3(RightPanel.transform.localScale.x, scaleYMultiplier + 4, RightPanel.transform.localScale.z);
        MiddlePanel.transform.localScale = new Vector3(0.1F * scaleYMultiplier + 0.2F,MiddlePanel.transform.localScale.y, MiddlePanel.transform.localScale.z);

    }
}
