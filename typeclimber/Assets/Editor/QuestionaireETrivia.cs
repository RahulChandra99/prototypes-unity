using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(QBTrivia))]
public class QuestionaireETrivia : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        QBTrivia question = (QBTrivia)target;
        if (GUILayout.Button("Save To DataBase"))
        {
            //question.saveToFile();
        }
        if (GUILayout.Button("Load From DataBase"))
        {
            //question.LoadFromFile();
        }
    }
}