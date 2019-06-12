using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class PaintingList : MonoBehaviour
{
    public PaintingListImage PaintingListImagePrefab;
    public List<Painting> Paintings { set { paintings = value; InitImageList(); } }
    public VerticalLayoutGroup List;
    private List<GameObject> objects = new List<GameObject>();
    private List<Painting> paintings;
    public void InitImageList()
    {
        paintings.ForEach(p => {
            var listImage = Instantiate(PaintingListImagePrefab, List.transform);
            listImage.Painting = p;
            objects.Add(listImage.gameObject);
        });
    }
}
