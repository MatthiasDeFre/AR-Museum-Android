using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Doozy.Engine.SceneManagement;

public class StoryCell : MonoBehaviour
{
    public TextMeshProUGUI Title;
    public TextMeshProUGUI Description;
    public Image Image;
    public SelectedStory SelectedStory { get; set; }
    private string id;
    public void SetFields(string title, string description, Sprite sprite, string id)
    {
        Title.text = title;
        Description.text = description;
      
        this.id = id;
    }

    public void GoToStory()
    {
        SelectedStory?.Invoke(id);
    }
    public void UpdateImage(Sprite sprite)
    {
        Image.sprite = sprite;
    }
}
