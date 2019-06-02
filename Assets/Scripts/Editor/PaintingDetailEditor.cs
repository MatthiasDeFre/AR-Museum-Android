using Doozy.Engine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Editor.UI;
using UnityEditor;

[CustomEditor(typeof(PaintingDetailView))]
[CanEditMultipleObjects]
public class PaintingDetailEditor : UIViewEditor
{
    public override void OnInspectorGUI()
    {

        base.OnInspectorGUI();
        EditorGUILayout.PropertyField(GetProperty("Image"));
        EditorGUILayout.PropertyField(GetProperty("Title"));
        EditorGUILayout.PropertyField(GetProperty("Scroller"));
        EditorGUILayout.PropertyField(GetProperty("HeaderCellViewPrefab"));
        EditorGUILayout.PropertyField(GetProperty("RowCellViewPrefab"));
        EditorGUILayout.PropertyField(GetProperty("FooterCellViewPrefab"));
        EditorGUILayout.PropertyField(GetProperty("LockedPainting"));
        serializedObject.ApplyModifiedProperties();
    }
}
