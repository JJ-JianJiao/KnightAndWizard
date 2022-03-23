using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator anim;
    private GameObject attackTarget;

    //attack cooldown 
    private float lastAttackTime;
    private CharacterStats characterStats;
    private bool isDead;
    private bool isDeadPlayed;

    private float stopDistance;

    private void Awake()
    {
        isDeadPlayed = false;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        characterStats = GetComponent<CharacterStats>();

        //store the default stopDistance
        stopDistance = agent.stoppingDistance;
    }
    private void OnEnable()
    {
        //add the move and attack functions to MouseManager Events
        MouseManager.Instance.OnMouseClicked += MoveToTarget;
        MouseManager.Instance.OnEnemyClicked += EventAttack;
        //give the player characterStats to GameManager
        GameManager.Instance.RigisterPlayer(characterStats);
    }

    private void Start()
    {
        //SaveManager.Instance.LoadPlayerData();
    }

    private void OnDisable()
    {
        if (!MouseManager.IsInitialized) return;

        //when change the scene, the player will be disabled.
        //delete the current PlayerControllers methods in the singalton event
        MouseManager.Instance.OnMouseClicked -= MoveToTarget;
        MouseManager.Instance.OnEnemyClicked -= EventAttack;
    }

    private void Update()
    {
        isDead = characterStats.CurrentHealth == 0;

        //player is dead, notify all enemies
        if (isDead)
        {
            GameManager.Instance.NotifyObservers();
            if (!isDeadPlayed) {
                AudioManager.Instance.PlaySfx("PlayerDie");
                isDeadPlayed = true;
            }
        }

        SwitchAnimation();
        lastAttackTime -= Time.deltaTime;
    }

    private void SwitchAnimation() {
        anim.SetFloat("Speed", agent.velocity.sqrMagnitude);
        anim.SetBool("Death", isDead);
    }

    public void MoveToTarget(Vector3 target) {

        StopAllCoroutines();
        if (isDead) return;
        agent.stoppingDistance = stopDistance;
        agent.isStopped = false;
        agent.destination = target;
    }


    private void EventAttack(GameObject target)
    {
        if (isDead) return;
        if (target != null) {
            attackTarget = target;
            //detemin the critical attack
            characterStats.isCritical = UnityEngine.Random.value < characterStats.CriticalChance;
            StartCoroutine(MoveToAttackTarget());
        }
    }

    IEnumerator MoveToAttackTarget() {
        if (attackTarget != null)
        {
            agent.isStopped = false;
            //TODO: this attackRange is not perfect.
            //if the enemy is very big, this attackRange might be not engouh,
            //the player will still walk to the center of the enemy
            agent.stoppingDistance = characterStats.attackData.attackRange;
            transform.LookAt(attackTarget.transform);


            if (attackTarget.name.Contains("Rock")) { 
                
            }

            //keep moving to the target untill the target is in attackRange
            //while (Vector3.Distance(transform.position, attackTarget.transform.position) > characterStats.attackData.attackRange) {
            while (Vector3.Distance(transform.position, attackTarget.transform.position) > characterStats.AttackRange)
            {
                agent.destination = attackTarget.transform.position;
                yield return null;
            }


            //Attack
            agent.isStopped = true;
            if (lastAttackTime < 0)
            {
                anim.SetBool("Critical", characterStats.isCritical);
                anim.SetTrigger("Attack");

                lastAttackTime = characterStats.CoolDown;
            }
        }
    }

    //Animation event
    void Hit() {

        if (attackTarget.CompareTag("Attackable"))
        {
            //if (attackTarget.GetComponent<Rock>() && attackTarget.GetComponent<Rock>().rockStates == Rock.RockStates.HitNothing) {
            if (attackTarget.GetComponent<Rock>()) {
                attackTarget.GetComponent<Rock>().rockStates = Rock.RockStates.HitEnemy;

                //To advoid the velocity is samll and the rock state become HitNothing in Rock Update()
                attackTarget.GetComponent<Rigidbody>().velocity = Vector3.one;
                attackTarget.GetComponent<Rigidbody>().AddForce(transform.forward * 20, ForceMode.Impulse);
            }
        }
        else
        {
            var targetStats = attackTarget.GetComponent<CharacterStats>();
            targetStats.TakeDamage(characterStats, targetStats);
        }
        if(InventoryManager.Instance.equipmentData.items[0] != null)
        {
            if (InventoryManager.Instance.equipmentData.items[0].itemData == null)
                AudioManager.Instance.PlaySfx("Punch");
            else {
                if (InventoryManager.Instance.equipmentData.items[0].itemData.itemName == "Huge Sword")
                {
                    AudioManager.Instance.PlaySfx("TwoSword");
                }
                else if (InventoryManager.Instance.equipmentData.items[0].itemData.itemName == "Sword")
                {
                    AudioManager.Instance.PlaySfx("OneSword");
                }
            }

        }

    }

    public void StopMoving() {
        agent.destination = transform.position;
    }
}
