using UnityEngine;

/// <summary> 각 스테이지의 적 몬스터 프리팹 정보 </summary>
[CreateAssetMenu(fileName = "StageSettings", menuName = "Stage Settings")]
public class StageSettings : ScriptableObject {
    public GameObject monsterPrefab;

    // 필요시 보스 유닛인지, 인터벌을 다르게 줄것인지 정보 추가
    // 이후 배열로 수정
}