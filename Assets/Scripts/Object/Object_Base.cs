using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 모든 오브젝트들의 베이스. 스탯 등 공용 정보를 가짐 </summary>
public class Object_Base : MonoBehaviour
{
    // 각종 스탯
    public float currentHp;
    public float maxHp = 10.0f;
    public float attackPower = 10.0f;

    protected void Start() {
        currentHp = maxHp;
    }

    public virtual void OnAttack(float amount) {
        currentHp -= amount;
        if (currentHp <= 0) {
            currentHp = 0;
            Die();

            return;
        }

        UpdateHpBar();
    }

    public virtual void Die() {
        Destroy(gameObject);
    }

    public virtual void UpdateHpBar() {

    }
}
