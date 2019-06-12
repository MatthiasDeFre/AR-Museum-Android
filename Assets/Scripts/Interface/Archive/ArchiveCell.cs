using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Bachelorproef.Networking.FileDownload;

public class ArchiveCell : MonoBehaviour
{
    public TextMeshProUGUI Title;
    public TextMeshProUGUI Description;
    public Image Image;
    private StoryMeta story;
    public UpdateImageSprite UpdateImageSprite { get; set; }
    public SelectedArchive SelectedArchive { get; set; }
    public StoryMeta Story
    {
        get { return story; }
        set { story = value; InitFields(); }
    }


    private void InitFields()
    {
        Title.text = story.Title;
        Description.text = story.Description;
        Image.sprite = story.Sprite;
        story.UpdateImageSprite += UpdateImage;
    }
    private void UpdateImage(Sprite sprite)
    {
        Image.sprite = sprite;
    }
    public void OnSelected()
    {
        Debug.Log(story.Id);
        SelectedArchive?.Invoke(story);
    }
}
