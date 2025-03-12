using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> ���� �Ŵ��� ����, Singleton���� ���� </summary>
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
            spawnPoint = transform; // ���� ����Ʈ�� �ϴ� ������ ������Ʈ�� ��ġ��
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
