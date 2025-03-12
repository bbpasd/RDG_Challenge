using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using Unity.VisualScripting;
using UnityEngine;



/// <summary> 각종 매니져 관리, Singleton으로 동작 </summary>
public class Managers : MonoBehaviour {
    private static Managers _instance;

    #region Managers
    public static Managers Instance => Init();
    #endregion

    public static Player_Hero _player;
    public static Player_Hero Player {
        get {
            if (_player == null) {
                _player = FindObjectOfType<Player_Hero>();
            }
            return _player;
        }
    }

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