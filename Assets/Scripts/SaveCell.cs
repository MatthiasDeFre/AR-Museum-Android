using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveCell : MonoBehaviour
{
    public TextMeshProUGUI Title;
    public TextMeshProUGUI Description;
    public Image Image;
    public SelectedSave SelectedSave { get; set; }
    private string pathName;
    public void SetFields(string title, string description, Sprite sprite, string pathName)
    {
        Title.text = title;
        Description.text = description;
        Image.sprite = sprite;
        this.pathName = pathName;
    }

    public void GoToStory()
    {
        SelectedSave?.Invoke(pathName);
    }
}
