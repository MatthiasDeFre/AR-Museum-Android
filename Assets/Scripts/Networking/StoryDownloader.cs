using Bachelorproef;
using Bachelorproef.ObjectClasses;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public static class StoryDownloader
{
    public static IEnumerator DownloadStoryList(MainMenuController controller)
    {
      //  using (UnityWebRequest webRequest = UnityWebRequest.Get("http://localhost:3000/list"))
        using (UnityWebRequest webRequest = UnityWebRequest.Get("https://quiet-reef-16174.herokuapp.com/list"))
        { 
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                Debug.Log(webRequest.error);
            }
            else
            {
              
                var json = webRequest.downloadHandler.text;
                List<StoryMeta> stories = JsonConvert.DeserializeObject<List<StoryMeta>>(json);

               controller.DownloadListCallBack(stories);
            }
        }
    }
    public static IEnumerator DownloadArchivedStory(string storyId, ArchiveList archiveList)
    {
        Debug.Log("ID " + storyId);
        using (UnityWebRequest webRequest = UnityWebRequest.Get("https://quiet-reef-16174.herokuapp.com/" + storyId))
        {
            System.Random rnd = new System.Random();
            /*var text = new DownloadHandlerFile(Application.persistentDataPath + "/image" + rnd.Next(1,1000) +".png");
             webRequest.downloadHandler = text;*/
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                Debug.Log(webRequest.error);
            }
            else
            {
                Debug.Log("GOT");
                // var texture = new Texture2D(2, 2);
                var json = webRequest.downloadHandler.text;
                KnownTypesBinder knownTypesBinder = new KnownTypesBinder
                {
                    KnownTypes = new List<Type> { typeof(Riddle), typeof(RiddleFind), typeof(RiddleScan) }
                };
                Story newStockholder = JsonConvert.DeserializeObject<Story>(json, new JsonSerializerSettings
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.All,
                    TypeNameHandling = TypeNameHandling.Auto,
                    MetadataPropertyHandling = MetadataPropertyHandling.ReadAhead,
                    SerializationBinder = knownTypesBinder
                });

                newStockholder.Paintings.ForEach(p =>
                {
                    p.RiddlesBeforeUnlock.ForEach(r =>
                    {
                        Debug.Log("HINT " + r.Hint);
                        r.Paintings.Add(p);
                    });
                    p.RiddlesToAttachToImage.ForEach(r =>
                    {
                        if (r.GetType() == typeof(RiddleScan))
                        {
                            ((RiddleScan)r).PaintingsToScan.Add(p);
                        }
                    });
                });
                newStockholder.Id = storyId;
                archiveList.DownloadCallBack(newStockholder);
            }
        }
    }
    public static IEnumerator DownloadStory(string storyId, MainController controller)
    {
       // using (UnityWebRequest webRequest = UnityWebRequest.Get("localhost:3000/" + storyId))
        using (UnityWebRequest webRequest = UnityWebRequest.Get("https://quiet-reef-16174.herokuapp.com/" + storyId))
        {
            System.Random rnd = new System.Random();
            /*var text = new DownloadHandlerFile(Application.persistentDataPath + "/image" + rnd.Next(1,1000) +".png");
             webRequest.downloadHandler = text;*/
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                Debug.Log(webRequest.error);
            }
            else
            {
                Debug.Log("GOT");
                // var texture = new Texture2D(2, 2);
                var json = webRequest.downloadHandler.text;
                JObject jsonParsed = JObject.Parse(json);
                KnownTypesBinder knownTypesBinder = new KnownTypesBinder
                {
                    KnownTypes = new List<Type> { typeof(Riddle), typeof(RiddleFind), typeof(RiddleScan) }
                };
                Story newStockholder = JsonConvert.DeserializeObject<Story>(json, new JsonSerializerSettings
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.All,
                    TypeNameHandling = TypeNameHandling.Auto,
                    MetadataPropertyHandling = MetadataPropertyHandling.ReadAhead,
                    SerializationBinder = knownTypesBinder
                });
             
                newStockholder.Paintings.ForEach(p =>
                { 
                    p.RiddlesBeforeUnlock.ForEach(r =>
                    {
                        Debug.Log("HINT " + r.Hint);
                        r.Paintings.Add(p);
                    });
                    p.RiddlesToAttachToImage.ForEach(r =>
                    {
                        if (r.GetType() == typeof(RiddleScan))
                        {
                            ((RiddleScan)r).PaintingsToScan.Add(p);
                        }
                    });
                });
                controller.StoryDownloadCallback(newStockholder);
            }
        }
    }
}
