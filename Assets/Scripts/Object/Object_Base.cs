using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> ��� ������Ʈ���� ���̽�. ���� �� ���� ������ ���� </summary>
public class Object_Base : MonoBehaviour
{
    // ���� ����
    public float currentHp;
    public float maxHp = 10.0f;
    public float attackPower = 10.0f;

    private void Start() {
        currentHp = maxHp;
    }

    public virtual void OnAttack(float amount) {

    }
}
