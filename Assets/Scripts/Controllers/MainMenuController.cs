using Bachelorproef.Networking.FileDownload;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    private List<StoryMeta> stories;
    public SelectStory SelectStory;
    public ArchiveList ArchiveList;
    public StatOverview StatOverview;
    // Start is called before the first frame update
    void Start()
    {
        StatController.InitDict();
        StatOverview.Init();
       // StartCoroutine(FileDownloader.DownloadPDF("https://docs.wixstatic.com/ugd/9c32cf_97e6c68de74f4509b5adec44456ae718.pdf"));
        StartCoroutine(StoryDownloader.DownloadStoryList(this));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DownloadListCallBack(List<StoryMeta> list)
    {
        stories = list;
        stories.ForEach(s =>
        {
            StartCoroutine(FileDownloader.DownloadBanner(s));
        });
        SelectStory.Stories = list;
        ArchiveList.Stories = stories;
    }
}
