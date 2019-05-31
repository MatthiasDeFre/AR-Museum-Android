using Bachelorproef.Networking.FileDownload;
using GoogleARCore;
using GoogleARCoreInternal;
using System.Collections.Concurrent;
using System.Threading;
using Unity.Jobs;
using UnityEngine;
using Newtonsoft;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using Bachelorproef.ObjectClasses;
using System;
using GoogleARCore.Examples.AugmentedImage;
using Doozy.Engine.UI;
using Doozy.Engine.Progress;
using System.Collections;

namespace Bachelorproef
{
    public class MainController : MonoBehaviour
    {
        private ConcurrentQueue<Painting> databaseQueue = new ConcurrentQueue<Painting>();
        public ARCoreSession arcoreSession;
        public AugmentedImageExampleController AugmentedController;
        private AugmentedImageDatabase augmentedImageDatabase;
        public PaintingListScroller PaintingListScroller;
        public Progressor Progressor;
        private Story story;
        // Start is called before the first frame update
        void Start()
        {

            NewTonTest();
            //DownloadExhibitionData();
            
            // Loading = TRUE
        }

        // Update is called once per frames
        void Update()
        {
        }
        void NewTonTest()
        {
            StartCoroutine(StoryDownloader.DownloadStory("item1", this));
          
        }
        void DownloadExhibitionData()
        {
            augmentedImageDatabase = (AugmentedImageDatabase)ScriptableObject.CreateInstance(typeof(AugmentedImageDatabase));
            arcoreSession.SessionConfig.AugmentedImageDatabase = augmentedImageDatabase;
            /*ImageTarget img = new ImageTarget();
            ImageTarget img2 = gameObject.AddComponent<ImageTarget>();
            img.ImageUrl = "https://vuforialibrarycontent.vuforia.com/Images/leaves_5star_bg.jpg";
            img2.ImageUrl = "https://developers.google.com/ar/images/augmented-images-earth.jpg";

            StartCoroutine(FileDownloader.DownloadImage(img, databaseQueue, this));
            StartCoroutine(FileDownloader.DownloadImage(img2, databaseQueue, this));*/
        }

        void InitSceneWithData()
        {
            // Loading = FALSE
        }
        public void StoryDownloadCallback(Story story)
        {
            Progressor.SetProgress(0.20F);
            this.story = story;
            augmentedImageDatabase = (AugmentedImageDatabase)ScriptableObject.CreateInstance(typeof(AugmentedImageDatabase));
            arcoreSession.SessionConfig.AugmentedImageDatabase = augmentedImageDatabase;
           
            story.Paintings.ForEach(painting =>
            {
                Debug.Log("HASH-2 " + painting.GetHashCode());
                StartCoroutine(FileDownloader.DownloadImage(painting, databaseQueue, this));
            });
            var storyDict = story.Paintings.ToDictionary(painting => painting.SortOrder.ToString(), painting => painting);
            AugmentedController.Paintings = storyDict;
        }
        public void PopQueue()
        {
           lock(databaseQueue)
            {
                Painting painting = null;
                if (databaseQueue.TryDequeue(out painting))
                {
                    Progressor.SetProgress(Progressor.Progress + 0.4F / story.Paintings.Count);
                    Debug.Log(Progressor.Progress);
                    if (story.Paintings.All(p => p.Image != null)) PaintingListScroller.Paintings = story.Paintings;
                    Thread t2 = new Thread(delegate ()
                    {
                        var test2 = augmentedImageDatabase.AddImage(painting.SortOrder.ToString(), painting.Image);
                        Progressor.SetProgress(Progressor.Progress + 0.4F / story.Paintings.Count);
                        Debug.Log("AG " + augmentedImageDatabase.Count);
                        Debug.Log("SG " + story.Paintings.Count);
                        if (augmentedImageDatabase.Count == story.Paintings.Count) StartCoroutine(DestroyProgressor());
                    });
                    t2.Start();                    
                }
            }
        }
        IEnumerator DestroyProgressor()
        {
            yield return new WaitForSeconds(0.5F);
            Destroy(Progressor.gameObject);
        }
    }
}
