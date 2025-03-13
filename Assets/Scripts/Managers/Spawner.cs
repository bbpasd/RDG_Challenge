using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Spawner 싱글톤 Managers에서 분리 </summary>
public class Spawner : MonoBehaviour {
    private static Spawner _instance;

    public GameObject monsterPrefab;
    public Transform spawnPoint;
    public float spawnInterval = 2f;
    public float[] spawnHeights = new float[] {-3.22f, -3.72f, 2.72f }; // 스폰 y좌표 길 3개
    public int SPAWNWAY = 1;    // TEST 일단 길 1군데에서만 스폰됨

    public static Spawner Instance => Init();

    public static Spawner Init() {
        if (_instance == null) {
            _instance = FindObjectOfType<Spawner>();

            if (_instance == null) {
                GameObject singletonObject = new GameObject("@Spawner");
                _instance = singletonObject.AddComponent<Spawner>();
            }
        }
        _instance.InitInstance();

        return _instance;
    }

    private void InitInstance() {
        if (spawnPoint == null) {
            spawnPoint = transform; // 스폰 포인트는 일단 스포너 오브젝트의 위치로
        }
    }

    public void StartSpawn() {
        StartCoroutine(SpawnMonsters());
    }

    private IEnumerator SpawnMonsters() {
        while (true) {
            int spawnIndex = UnityEngine.Random.Range(1, SPAWNWAY + 1) - 1;
            Vector3 spawnPosition = new Vector3(spawnPoint.position.x, spawnHeights[spawnIndex], spawnPoint.position.z);
            Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
