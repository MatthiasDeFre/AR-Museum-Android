using Bachelorproef.Networking.FileDownload;
using Doozy.Engine.SceneManagement;
using Doozy.Engine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArchiveList : MonoBehaviour
{
    public ArchiveCell ArchiveCellPrefab;
    public GameObject Container;
    public SceneLoader SceneLoader;
    public PaintingListScroller PaintingList;
    public UIView Loading;
    public Dictionary<string, Story> storiesWithPaintings = new Dictionary<string, Story>();
    public Button Select;
    public Button Load;
    public Button Pdf;
    private Story selectedStory;
    private string selectedSavePath;
    private string selectedPdf;
    private List<StoryMeta> stories;
    private int counter;

    public List<StoryMeta> Stories
    {
        get { return stories; }
        set { stories = value; InitContainer(); }
    }
    void Start()
    {
        var rect = PaintingList.transform.Find("Container").GetComponent<RectTransform>();
        Debug.Log("Container " + rect);
        rect.offsetMax = Vector2.zero;
        rect.offsetMin = new Vector2(0, 300);

    }
    private void InitContainer()
    {
        stories.ForEach(s =>
        {
            var cell = Instantiate(ArchiveCellPrefab);
            cell.Story = s;
            cell.transform.parent = Container.transform;
            cell.transform.localScale = Vector3.one;
            cell.SelectedArchive = StorySelected;
        });
    }
    private void StorySelected(StoryMeta storyMeta)
    {
        Select.interactable = true;
        Pdf.interactable = true;
      
        selectedSavePath = null;
        selectedPdf = storyMeta.PdfUrl;
        Loading.Show();
        counter = 0;
        Story story;
        var save = SaveController.Saves.Find(s => s.StoryId == storyMeta.Id);
        if(save != null)
        {
            selectedSavePath = save.PathName;
            Load.interactable = true;
        } else
        {
            Load.interactable = false;
        }
        if (storiesWithPaintings.TryGetValue(storyMeta.Id, out story))
        {
            selectedStory = story;
            InitDetail();
        }
        else {
           
            if(save != null)
            {
                Debug.Log("Save found " + save.Date.ToLongDateString());
                selectedStory = SaveController.LoadStory(save.PathName);
                storiesWithPaintings.Add(selectedStory.Id, selectedStory);
                InitDetail();
            } else
            {
                StartCoroutine(StoryDownloader.DownloadArchivedStory(storyMeta.Id, this));
            }
        }
  
    }

    public void DownloadCallBack(Story story)
    {
        Debug.Log("DOWNLOAD CALLBACK" + story.Paintings.Count);
        selectedStory = story;
        storiesWithPaintings.Add(story.Id, story);
        story.Paintings.ForEach(p =>
        {
            StartCoroutine(FileDownloader.DownloadPainting(p, this));
        }); 
    }
    public void DownloadPaintingCallback()
    {
       
        if (++counter == selectedStory.Paintings.Count) InitDetail();
    }
    private void InitDetail()
    {
        Loading.Hide();
        var rect = PaintingList.PaintingDetailView.GetComponent<RectTransform>();
        PaintingList.Paintings = selectedStory.Paintings;
        rect.offsetMax = new Vector2(0, 0);
        rect.offsetMin = new Vector2(0, 300);

    }
    public void LoadSelected()
    {
        GlobalVariables.IsNew = true;
        GlobalVariables.StoryId = selectedStory.Id;
        SceneLoader.SetSelfDestructAfterSceneLoaded(true);
        SceneLoader.LoadSceneAsync();
    }
    public void LoadSave()
    {
        GlobalVariables.IsNew = false;
        GlobalVariables.SavePathName= selectedSavePath;
        SceneLoader.SetSelfDestructAfterSceneLoaded(true);
        SceneLoader.LoadSceneAsync();
    }
    public void DownloadPdf()
    {
        StartCoroutine(FileDownloader.DownloadPDF(selectedPdf));
    }
}
