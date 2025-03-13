using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Spawner �̱��� Managers���� �и� </summary>
public class Spawner : MonoBehaviour {
    private static Spawner _instance;

    public GameObject monsterPrefab;
    public Transform spawnPoint;
    public float spawnInterval = 2f;
    public float[] spawnHeights = new float[] {-3.22f, -3.72f, 2.72f }; // ���� y��ǥ �� 3��
    public int SPAWNWAY = 1;    // TEST �ϴ� �� 1���������� ������

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
            int spawnIndex = UnityEngine.Random.Range(1, SPAWNWAY + 1) - 1;
            Vector3 spawnPosition = new Vector3(spawnPoint.position.x, spawnHeights[spawnIndex], spawnPoint.position.z);
            Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
