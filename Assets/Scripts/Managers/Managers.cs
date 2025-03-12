using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using Unity.VisualScripting;
using UnityEngine;



/// <summary> ���� �Ŵ��� ����, Singleton���� ���� </summary>
public class Managers : MonoBehaviour {
    private static Managers _instance;

    #region Managers
    public static Managers Instance => Init();
    #endregion

    private static Managers Init() {
        if (_instance == null) {
            _instance = FindObjectOfType<Managers>();

            if (_instance == null) {
                GameObject singletonObject = new GameObject("@Managers");
                _instance = singletonObject.AddComponent<Managers>();
            }
        }
        return _instance;
    }
}