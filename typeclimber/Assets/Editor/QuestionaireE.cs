using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(QuestionBank))]
public class QuestionaireE : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        QuestionBank question = (QuestionBank)target;
        if (GUILayout.Button("Save To DataBase"))
        {
            question.saveToFile();
        }
        if (GUILayout.Button("Load From DataBase"))
        {
            question.LoadFromFile();
        }
    }
}
