using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CharacterStats))]
public class Golem : EnemyController
{
    [Header("Skill")]
    public float kickForce = 30f;
    public GameObject rockPrefab;
    public Transform HandPos;

    //Animation Event
    public void KickOff() {
        if (attackTarget != null && transform.IsFacingTarget(attackTarget.transform))
        {
            var targetStats = attackTarget.GetComponent<CharacterStats>();

            var kickOffDirection = attackTarget.transform.position - transform.position;
            kickOffDirection.Normalize();
            attackTarget.GetComponent<NavMeshAgent>().isStopped = true;
            attackTarget.GetComponent<NavMeshAgent>().velocity = kickForce * kickOffDirection;
            attackTarget.GetComponent<Animator>().SetTrigger("Dizzy");
            targetStats.TakeDamage(characterStats, targetStats);
        }
    }

    //Animation Event
    public void ThrowRock() {
        var rock = Instantiate(rockPrefab, HandPos.position, Quaternion.identity);
        if (attackTarget != null) {
            //var rock = Instantiate(rockPrefab, HandPos.position, Quaternion.identity);
            rock.GetComponent<Rock>().target = attackTarget;
        }
    }
}
