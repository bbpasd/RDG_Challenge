using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary> Base of ALL Object </summary>
public class Object_Base : MonoBehaviour
{
    // ∞¢¡æ Ω∫≈»
    public float currentHp;
    public float maxHp = 10.0f;
    public float attackPower = 10.0f;
    public float attackInterval = 1.0f;
    public bool isAttackedOnShot = false;
    public bool isAttacking = false;
    public Object_Base attackTarget;

    public GameObject hpPanel;
    public Slider hpSlider;

    private bool isDie = false;

    protected void Start() {
        currentHp = maxHp;

        hpPanel = transform.Find("HPPanel")?.gameObject;

    }

    public virtual void Attack() {
        attackTarget.OnAttack(attackPower);
    }

    public virtual void OnAttack(float amount) {
        currentHp -= amount;
        if (currentHp <= 0 && !isDie) {
            currentHp = 0;
            isDie = true;
            Die();

            return;
        }

        if (!isAttackedOnShot) {
            isAttackedOnShot = true;
            hpPanel.SetActive(true);
            hpSlider = hpPanel.transform.Find("Canvas")?.Find("HpSlider")?.GetComponent<Slider>();
            
        }
        UpdateHpBar();
    }

    public virtual void AttackEvent() {
        Attack();
    }

    public virtual void Die() {
        Destroy(gameObject);
    }

    public virtual void UpdateHpBar() {
        if (hpSlider == null) return;

        hpSlider.value = currentHp / maxHp;
    }
}
