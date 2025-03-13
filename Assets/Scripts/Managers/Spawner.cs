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
    public float[] spawnHeights = new float[] {-3.22f, -3.72f, -2.72f }; // ���� y��ǥ �� 3��
    public int SPAWNWAY = 1;    // TEST �ϴ� �� 1���������� ������
    public string[] spawnLayers = new string[] { "MonsterWay1", "MonsterWay2", "MonsterWay3" };

    public Transform monsterParent;

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
            monsterParent = new GameObject("@Monster").transform;
        }
    }

    public void StartSpawn() {
        StartCoroutine(SpawnMonsters());
    }

    private IEnumerator SpawnMonsters() {
        while (true) {
            int spawnWay = UnityEngine.Random.Range(1, SPAWNWAY + 1);
            int spawnIndex =  spawnWay - 1;
            
            Vector3 spawnPosition = new Vector3(spawnPoint.position.x, spawnHeights[spawnIndex], spawnPoint.position.z);
            GameObject monster = Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);
            monster.GetComponent<Monster_Base>().spawnWay = spawnWay;
            monster.layer = LayerMask.NameToLayer(spawnLayers[spawnIndex]);
            monster.transform.parent = monsterParent;

            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
