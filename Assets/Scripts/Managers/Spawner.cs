using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 각종 매니져 관리, Singleton으로 동작 </summary>
public class Spawner : MonoBehaviour {
    private static Spawner _instance;

    public GameObject monsterPrefab;
    public Transform spawnPoint;
    public float spawnInterval = 2f;

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
            Instantiate(monsterPrefab, spawnPoint.position, Quaternion.identity);
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
