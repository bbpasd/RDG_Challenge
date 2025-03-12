using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Monster_ZombieMelee : Monster_Base
{
    private void Update() {
        MoveToPlayer();
    }

    private void MoveToPlayer() {
        //Vector3 direction = (Managers.Player.transform.position - transform.position).normalized;
        Vector3 playerPos = Managers.Player.transform.position;
        Vector3 targetPos = new Vector3(playerPos.x, transform.position.y, playerPos.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
    }
}
