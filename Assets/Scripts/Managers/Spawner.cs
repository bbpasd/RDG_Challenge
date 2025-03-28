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
    public int SPAWNWAY_NUM = 3;    // TEST용값 = 1
    public float[] spawnHeights = new float[] {-2.72f, -3.22f, -3.72f }; // 스폰 y좌표 길 3개. 아래 변수들은 일단 전부 직접 3개씩 작성
    public string[] spawnLayers = new string[] { "MonsterWay1", "MonsterWay2", "MonsterWay3" };

    private int monsterCount = 0;
    public Transform[] monsterParent;   // 각 라인의 몬스터들을 배치할 곳, 

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
            monsterParent = new Transform[] {
                new GameObject("@Monster1").transform,
                new GameObject("@Monster2").transform,
                new GameObject("@Monster3").transform
            };
        }
    }

    public void StartSpawn() {
        StartCoroutine(SpawnMonsters());
    }

    private IEnumerator SpawnMonsters() {
        yield return new WaitForSeconds(3.0f); // 최초 대기 시간
        while (true) {
            int spawnWay = UnityEngine.Random.Range(1, SPAWNWAY_NUM + 1);
            int spawnIndex =  spawnWay - 1;
            
            Vector3 spawnPosition = new Vector3(spawnPoint.position.x, spawnHeights[spawnIndex] +0.1f, spawnPoint.position.z);
            GameObject monster = Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);
            monster.GetComponent<Monster_Base>().spawnWay = spawnWay;
            monster.layer = LayerMask.NameToLayer(spawnLayers[spawnIndex]);
            monster.transform.parent = monsterParent[spawnIndex];
            monster.name = monster.name + monsterCount++;
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}