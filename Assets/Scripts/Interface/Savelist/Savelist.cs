using Doozy.Engine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Savelist : MonoBehaviour
{
    private List<StorySave> saves;
    public SaveCell SaveCellPrefab;
    public GameObject Container;
    public SceneLoader SceneLoader;
    public List<StorySave> Saves
    {
        get { return saves; }
        set { saves = value; InitSavelist(); }
    }

    // Start is called before the first frame update
    void Start()
    {
        InitSavelist();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void InitSavelist()
    {
        saves = SaveController.Saves;
        saves.ForEach(s =>
        {
            var saveObject = Instantiate(SaveCellPrefab);
            saveObject.SetFields(s.StoryName, s.Date.ToLongDateString(), null, s.PathName);
            saveObject.SelectedSave = SaveSelected;
            saveObject.transform.parent = Container.transform;
            saveObject.transform.localScale = new Vector3(1, 1);
        });
    }
    private void SaveSelected(string pathName)
    {
        GlobalVariables.IsNew = false;
        GlobalVariables.SavePathName = pathName;
        SceneLoader.SetSelfDestructAfterSceneLoaded(true);
        SceneLoader.LoadSceneAsync();
    }
}
