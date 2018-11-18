﻿using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WalkPath3))]
public class WalkPath3Editor : Editor
{

    public override void OnInspectorGUI()
    {
        //Debug.Log(1);
        WalkPath3 _WalkPath = target as WalkPath3;


        DrawDefaultInspector();


        if (GUILayout.Button("Populate!"))
        {
            if (_WalkPath.par != null)
                DestroyImmediate(_WalkPath.par);

            if (_WalkPath.peoplePrefabs != null && _WalkPath.peoplePrefabs.Length > 0 && _WalkPath.peoplePrefabs[0] != null)
            {
                _WalkPath.CalPath();
                _WalkPath.SpawnPeople();
            }
        }

            if (GUILayout.Button("Remove people"))
            {
                if (_WalkPath.par != null)
                    DestroyImmediate(_WalkPath.par);
            }

            EditorGUILayout.Space();


            if (_WalkPath.peoplePrefabs == null || _WalkPath.peoplePrefabs.Length == 0 || _WalkPath.peoplePrefabs[0] == null)
                EditorGUILayout.HelpBox("To create a path must be at least 1 people prefab", MessageType.Warning);

        }
    }
