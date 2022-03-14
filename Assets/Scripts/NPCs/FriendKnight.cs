using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FriendKnight : MonoBehaviour
{
    public GameObject followTarget;
    public GameObject attackTarget;

    public bool isActive;
    public NavMeshAgent agent;
    public NavMeshObstacle navMO;

    public Animator anim;

    private void Awake()
    {
        isActive = false;
        agent = GetComponent<NavMeshAgent>();
        navMO = GetComponent<NavMeshObstacle>();
        anim = GetComponent<Animator>();

    }

    private void Start()
    {
        if (isActive)
        {
            MouseManager.Instance.OnEnemyClicked += GetAttackTarget;
            //MouseManager.Instance.OnMouseClicked += FollowPlayerMovement;
        }
    }

    private void OnDestroy()
    {
        if (isActive)
        {
            MouseManager.Instance.OnEnemyClicked -= GetAttackTarget;
            //MouseManager.Instance.OnMouseClicked -= FollowPlayerMovement;
        }
    }
    //private void FollowPlayerMovement(Vector3 obj)
    //{
    //    attackTarget = null;
    //}

    private void GetAttackTarget(GameObject target)
    {
        if(target.CompareTag("Enemy"))
            attackTarget = target;
    }

    private void Update()
    {
        if (isActive && Vector3.Distance(followTarget.transform.position, transform.position) > 8) {
            attackTarget = null;
        }

        if (isActive && followTarget != null && attackTarget == null) {
            agent.stoppingDistance = 3;
            agent.SetDestination(followTarget.transform.position);
            Debug.Log(Vector3.Distance(transform.position, followTarget.transform.position));
        }

        if (isActive && attackTarget != null) {
            if (attackTarget.GetComponent<EnemyController>() && attackTarget.GetComponent<EnemyController>().IsDead != true)
            {
                agent.stoppingDistance = 1;
                agent.SetDestination(attackTarget.transform.position);
                anim.SetFloat("Speed", 1);
                anim.SetBool("Attack", false);
                if (Vector3.Distance(attackTarget.transform.position, transform.position) < 3)
                {
                    transform.LookAt(attackTarget.transform);
                    agent.SetDestination(transform.position);
                    anim.SetFloat("Speed", 0);
                    anim.SetBool("Attack", true);
                }

            }
        }

        SwitchAnimation();
    }

    private void SwitchAnimation()
    {
        if (agent.enabled && Vector3.Distance(transform.position, followTarget.transform.position) <= 3 && attackTarget == null)
        {
            anim.SetFloat("Speed", 0);
        }
        else if(agent.enabled && !agent.isStopped && attackTarget == null)
        {
            anim.SetFloat("Speed", 1);
        }
    }

    private void OnMouseUp()
    {

        if (Vector3.Distance(transform.position, GameManager.Instance.playerStates.transform.position) < 2)
        {
            Debug.Log("I will join you, my friend");
            isActive = true;
            followTarget = GameManager.Instance.playerStates.gameObject;
            LevelManager.Instance.activeFriendKnight = true;
            navMO.enabled = false;
            agent.enabled = true;
        }
    }

    public void SetFollowTarget(GameObject obj) {
        followTarget = obj;
        isActive = true;
        navMO.enabled = false;
        agent.enabled = true;
    }

    void Hit()
    {
        var targetStats = attackTarget.GetComponent<CharacterStats>();
        targetStats?.TakeDamage(8, targetStats);
    }
}
