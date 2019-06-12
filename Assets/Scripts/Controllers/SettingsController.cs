using Doozy.Engine.SceneManagement;
using Doozy.Engine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsController : MonoBehaviour
{
    private UIView view;
    public SceneLoader SceneLoader;
    public Story Story { get; set; }
    public void Save()
    {
        SaveController.SaveStory(Story);

    }
    public void Exit()
    {
        StatController.InitDict();
        SceneLoader.LoadSceneAsync();
    }
    public void Show()
    {
        if (view == null)
        {
            view = GetComponent<UIView>();
        }
        view.Show();
    }
}
