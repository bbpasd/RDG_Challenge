using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float attackPower = 1.0f;
    public float lifetime = 5.0f;

    private void Start() {
        StartCoroutine(DestroyBullet());
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Monster")) {
            collision.gameObject.GetComponent<Monster_Base>().OnAttack(attackPower);
            Destroy(gameObject);
        }
    }

    private IEnumerator DestroyBullet() {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}
