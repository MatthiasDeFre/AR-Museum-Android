using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void SelectedStory(string id);
public delegate void SelectedSave(string pathName);
public delegate void SelectedArchive(StoryMeta story);
public delegate void SelectedPlaneDetection(int index);
public delegate void SelectedPainting(Painting painting);
public delegate void SelectedChangedDelegate(bool val);

public delegate void UpdateImageSprite(Sprite sprite);

