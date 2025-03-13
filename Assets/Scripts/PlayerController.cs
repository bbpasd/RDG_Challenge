using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public GameObject gunObject;
    public GameObject bulletPrefab;
    public float bulletSpeed = 3.0f;
    public float bulletDamage;
    public int bulletNum = 5;
    public float bulletSpreadAngle = 10f;
    public float shootInterval = 1.0f;

    public bool gameRunning = true; // temp

    public bool isClick = false;

    private float gunRotateWeight = 30; // 총 이미지가 기본적으로 위로 올라가있어서 보정해주는 값

    public Transform bulletParent;

    private void Start() {
        bulletDamage = GetComponent<Player_Hero>().attackPower;

        bulletParent = GameObject.Find("@Bullet")?.transform;
        if (bulletParent == null) {
            bulletParent = new GameObject("@Bullet").transform;
        }
        StartCoroutine(StartShoot());
    }

    private void Update() {
        //if (Input.GetMouseButtonDown(0)) {
        //    OnMouseClick();
        //}
    }

    private void OnMouseClick() {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        Debug.Log("마우스 클릭 위치: " + mousePos);
    }

    private IEnumerator StartShoot() {
        while (true) {
            if (!gameRunning) break;
            Vector3 target = Vector3.zero;
            if (isClick) {

            }
            else {
                target = FindClosestTarget();
                if (target != null) {
                    Shoot(target);
                }
            }

            yield return new WaitForSeconds(shootInterval);
        }
    }

    private Vector3 FindClosestTarget() {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        Vector3 closestTarget = Vector3.zero;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject monster in monsters) {
            float distance = Vector2.Distance(transform.position, monster.transform.position);
            if (distance < closestDistance) {
                closestTarget = monster.transform.position;
                closestDistance = distance;
            }
        }

        RotateGunToTarget(closestTarget);
        return closestTarget;
    }

    private void Shoot(Vector3 targetPos) {
        Vector2 direction = (targetPos - transform.position).normalized;

        // 총 종류에 따라 여러발 발사
        for (int i = 0; i < bulletNum; i++) {
            float angle = Random.Range(-bulletSpreadAngle, bulletSpreadAngle);
            Vector2 bulletDirection = Quaternion.Euler(0f, 0f, angle) * direction;

            GameObject bullet = Instantiate(bulletPrefab, gunObject.transform.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().velocity = bulletDirection * bulletSpeed;
            bullet.GetComponent<Bullet>().attackPower = bulletDamage;
            bullet.transform.parent = bulletParent;
        }

        // Shoot Animation
    }


    private void RotateGunToTarget(Vector3 targetPos) {
        if (gunObject == null) return;
        Vector2 direction = (targetPos - gunObject.transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        gunObject.transform.rotation = Quaternion.Euler(0f, 0f, angle - gunRotateWeight);
    }
}
