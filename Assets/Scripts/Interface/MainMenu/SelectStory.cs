using Doozy.Engine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectStory : MonoBehaviour
{
    public StoryCell StoryCellPrefab;
    public GameObject Container;
    public SceneLoader SceneLoader;
    private List<StoryMeta> stories;

    public List<StoryMeta> Stories
    {
        get { return stories; }
        set { stories = value; InitContainer(); }
    }

    private void InitContainer()
    {
        stories.ForEach(s =>
        {
            var cell = Instantiate(StoryCellPrefab);
            cell.SetFields(s.Title, s.Description, s.Sprite, s.Id);
            cell.SelectedStory = StorySelected;
            cell.transform.parent = Container.transform;
            cell.transform.localScale = Vector3.one;
            s.UpdateImageSprite += cell.UpdateImage;
        });
    }
    private void StorySelected(string id)
    {
        GlobalVariables.IsNew = true;
        GlobalVariables.StoryId = id;
        SceneLoader.SetSelfDestructAfterSceneLoaded(true);
        SceneLoader.LoadSceneAsync();
    }
   
}
