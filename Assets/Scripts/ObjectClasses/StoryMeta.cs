using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryMeta
{
    public string Id { get; set; }
    public string Url { get; set; }
    public string PdfUrl { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    private Sprite sprite;

    public Sprite Sprite
    {
        get { return sprite; }
        set { sprite = value;UpdateImageSprite?.Invoke(sprite); }
    }

    public UpdateImageSprite UpdateImageSprite;
  
}
