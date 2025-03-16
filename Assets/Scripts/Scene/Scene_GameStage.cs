using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 각 GameStage마다 배치 </summary>
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
