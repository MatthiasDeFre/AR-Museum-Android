using Bachelorproef.ObjectClasses;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;
public static class SaveController
{
    public static bool IsDirty { get; set; }
    private static List<StorySave> saves;
    private static string savePath = $"{Application.persistentDataPath}/saves.json";
    public static List<StorySave> Saves
    {
        get {
            if (saves == null || IsDirty)
            {
                IsDirty = false;
                try
                {
                    string json = File.ReadAllText(savePath);
                    saves = JsonConvert.DeserializeObject<List<StorySave>>(json);
                    saves.Sort((s, s2) => {
                        if (s.Date.Equals(s2.Date)) return 0;
                        if (s.Date < s2.Date) return 1;
                        return -1;
                    });
                }
                catch (Exception e)
                {
                    saves = new List<StorySave>();
                }
            }
            return saves;
        }
        set { saves = value; }
    }


    public static string SaveStory(Story story)
    {

        KnownTypesBinder knownTypesBinder = new KnownTypesBinder
        {
            KnownTypes = new List<Type> { typeof(Riddle), typeof(RiddleFind), typeof(RiddleScan) }
        };
        var json  = JsonConvert.SerializeObject(story, new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            PreserveReferencesHandling = PreserveReferencesHandling.All,
            TypeNameHandling = TypeNameHandling.Auto,
            SerializationBinder = knownTypesBinder
        });
        var pathName = $"{Application.persistentDataPath}/Story-{DateTime.Now.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds}.json";
        File.WriteAllText(pathName, json);
        AddSaveToList(pathName, story.Id, story.Title);
        IsDirty = true;
        StatController.PersistDict();
        return pathName;

    }
    public static Story LoadStory(string savePath)
    {
            var jsonString = File.ReadAllText(savePath);
       
            KnownTypesBinder knownTypesBinder = new KnownTypesBinder
            {
                KnownTypes = new List<Type> { typeof(Riddle), typeof(RiddleFind), typeof(RiddleScan) }
            };
            Story story = JsonConvert.DeserializeObject<Story>(jsonString, new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.All,
                TypeNameHandling = TypeNameHandling.Auto,
                MetadataPropertyHandling = MetadataPropertyHandling.ReadAhead,
                SerializationBinder = knownTypesBinder
            });
        story.Paintings.ForEach(p =>
        {
            Debug.Log("PAINTING PATH " + p.PathName);
            Debug.Log("PAINTING SAT " + p.Scanned);
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
            Texture2D texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            texture.LoadImage(File.ReadAllBytes(p.PathName));

            p.Image = texture;
            p.Sprite = Sprite.Create(texture, new Rect(0.0F, 0.0F, texture.width, texture.height), new Vector2(0.5F, 0.5F), 100F);
        });
            return story;
        }
    private static void AddSaveToList(string pathName, string storyId, string storyName)
    {
        List<StorySave> storySaves;
        try
        {
            string json = File.ReadAllText(savePath);
            storySaves = JsonConvert.DeserializeObject<List<StorySave>>(json);
        } catch (Exception e)
        {
            storySaves = new List<StorySave>();
        }
        storySaves.Add(new StorySave() { Date = DateTime.Now, PathName = pathName, StoryId = storyId, StoryName = storyName });
        var jsonString = JsonConvert.SerializeObject(storySaves);
        File.WriteAllText(savePath, jsonString);
    }
}

