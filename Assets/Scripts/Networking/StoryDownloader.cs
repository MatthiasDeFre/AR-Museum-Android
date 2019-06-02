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


                /*newStockholder.Riddles.ForEach(r =>
                {
                    if (r.GetType() == typeof(RiddleFind))
                    ((RiddleFind)r).PaintingsWithLocation.ForEach(loc => Debug.Log("VECTOR " + loc.StartLoc + " " + loc.EndLoc + " " + loc.PaintingId));
                r.PaintingIds.ForEach(id =>
                    {
                        r.Paintings.Add(newStockholder.Paintings.Find(p => p.SortOrder == id));
                        Debug.Log("CC" + r.Paintings.Count);
                    });
                  
                });*/
             
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
                /*string title = jsonParsed["title"].ToString();
                string description = jsonParsed["description"].ToString();
                List<JToken> images = jsonParsed["images"].Children().ToList();
                Dictionary<int, int[][]> temp_riddleIds = new Dictionary<int, int[][]>();
                Debug.Log(images.Count);
                images.ForEach(image =>
                {
                    int[][] temp_array = new int[2][];
                  
                    temp_array[0] = image["riddlesBeforeUnlock"].ToObject<int[]>();
                    temp_array[1] = image["riddlesToAttachToImage"].ToObject<int[]>();
                    temp_riddleIds.Add(image.Value<int>("id"), temp_array);
                    Debug.Log("loop");
                    Debug.Log(string.Join(",", temp_array[0]));

                });*/
            }
        }
    }
}
