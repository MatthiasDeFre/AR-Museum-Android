using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Concurrent;
using System;
using UnityEngine.Networking;

namespace Bachelorproef.Networking.FileDownload
{
    public static class FileDownloader
    { 
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
                    databaseQueue.Enqueue(painting);
                    controller.PopQueue();
                }
            }
        }
    }

}
