using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

namespace UnityEditor
{
    [CustomEditor(typeof(CustomScrollRect))]
    public class CustomScrollRectEditor : ScrollRectEditor
    {
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();

            CustomScrollRect targetScroll = (CustomScrollRect)target;
            targetScroll.LoadIconPrefab = (LoadIcon)EditorGUILayout.ObjectField("Load Icon Prefab", targetScroll.LoadIconPrefab, typeof(LoadIcon), true);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RegisterCompleteObjectUndo(targetScroll, "Change reference of object");
            }

            base.OnInspectorGUI();
        }
    }
}
