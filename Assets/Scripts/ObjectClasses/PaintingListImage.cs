using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Text.RegularExpressions;

public class PaintingListImage : MonoBehaviour
{
    private Painting painting;
    public Image Image;
    public TextMeshProUGUI Info;
    public TextMeshProUGUI NameAndDate;
    private bool hidden;
    void Update()
    {
        if(hidden && painting.Unlocked)
        {
            hidden = false;
            InitPainting();
        }
    }
    public Painting Painting
    {
        set { painting = value;InitPainting(); }
    }

    void InitPainting()
    {
        if(painting.Unlocked)
        {
            Image.sprite = Sprite.Create(painting.Image, new Rect(0, 0, painting.Image.width, painting.Image.height), new Vector2(0.5f, 0.5f));
            Info.text = string.Join("\n", painting.Info);
            NameAndDate.text = $"{painting.Date}: {painting.Name}";
            hidden = false;
        } else
        {
            Info.text = "Om dit schilderij te ontgrendelen moet je eerst het voorafgaande schilderij scannen!";
            NameAndDate.text = Regex.Replace($"{painting.Date}: {painting.Name}", "[A-Za-z0-9]", "?");
            hidden = true;
        }

    }
}
