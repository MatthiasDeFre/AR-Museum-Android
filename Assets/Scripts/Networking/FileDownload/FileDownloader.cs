using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Concurrent;
using System;
using UnityEngine.Networking;
using System.IO;
using UnityEngine.UI;

namespace Bachelorproef.Networking.FileDownload
{
    public static class FileDownloader
    { 
        public static IEnumerator DownloadBanner(StoryMeta storyMeta)
        {
            using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(storyMeta.Url))
            {
                yield return webRequest.SendWebRequest();
                var texture = DownloadHandlerTexture.GetContent(webRequest);

                texture.LoadImage(webRequest.downloadHandler.data);
                // Creating a sprite is slow, to make sure the ui thread won't be slowed down we will create it here 
                // => impact on memory but much better framerate
                Debug.Log("T " + storyMeta.Sprite);
                storyMeta.Sprite = Sprite.Create(texture, new Rect(0.0F, 0.0F, texture.width, texture.height), new Vector2(0.5F, 0.5F), 100F);
                Debug.Log("T2 " + storyMeta.Sprite);
                Debug.Log("SPRIT");
            }
        }
        public static IEnumerator DownloadPainting(Painting painting, ArchiveList archiveList)
        {
            Debug.Log("PAINTING ");
            using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(painting.Url))
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
                    var texture = DownloadHandlerTexture.GetContent(webRequest);

                    texture.LoadImage(webRequest.downloadHandler.data);
                    painting.Image = texture;

                    // Creating a sprite is slow, to make sure the ui thread won't be slowed down we will create it here 
                    // => impact on memory but much better framerate
                    painting.Sprite = Sprite.Create(texture, new Rect(0.0F, 0.0F, texture.width, texture.height), new Vector2(0.5F, 0.5F), 100F);
                    archiveList.DownloadPaintingCallback();
                }
            }
        }
        public static IEnumerator DownloadImage(Painting painting, ConcurrentQueue<Painting> databaseQueue, MainController controller)
        {
            using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(painting.Url))
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
                    var texture = DownloadHandlerTexture.GetContent(webRequest);
                  
                    texture.LoadImage(webRequest.downloadHandler.data);
                    painting.Image= texture;

                    // Creating a sprite is slow, to make sure the ui thread won't be slowed down we will create it here 
                    // => impact on memory but much better framerate
                    painting.Sprite = Sprite.Create(texture, new Rect(0.0F, 0.0F, texture.width, texture.height), new Vector2(0.5F, 0.5F), 100F);
                    Debug.Log("HASH-3 " + painting.GetHashCode());
                    painting.PathName = SaveToFile(painting.Name, texture.EncodeToJPG());
                    databaseQueue.Enqueue(painting);
                    controller.PopQueue();
                }
            }
        }
        
        public static IEnumerator DownloadPDF(string url)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {
                yield return webRequest.SendWebRequest();
                Debug.Log("DOWNLOAD " + webRequest.downloadHandler.data.Length);
                string pathName = $"{Application.persistentDataPath}/{DateTime.Now.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds}-{"PDF"}.pdf";
                File.WriteAllBytes(pathName, webRequest.downloadHandler.data);
            } 
        }

        private static string SaveToFile(string name, byte[] textureData)
        {
            string pathName = $"{Application.persistentDataPath}/{DateTime.Now.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds}-{name}.bytes";
            Debug.Log("PATH " + pathName);
            File.WriteAllBytes(pathName, textureData);
            return pathName;
        }

    }
 

}
