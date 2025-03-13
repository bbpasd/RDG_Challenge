using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> �ΰ��ӿ��� �۵��� Scene ��ũ��Ʈ </summary>
public class Scene_GameStage : Scene_Base
{
    public Spawner spawner;
    public StageSettings stageSettings;

    private void Awake() {
        if (stageSettings == null) {
            Debug.LogError($"Stage Settings not found.");
            return;
        }

        Spawner.Init();
        Spawner.Instance.monsterPrefab = stageSettings.monsterPrefab;
        Spawner.Instance.StartSpawn();
    }
}
