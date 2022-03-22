using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyStates { GUARD, PATROL, CHASE, DEAD}
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CharacterStats))]
public class EnemyController : MonoBehaviour, IEndGameObsever
{
    private EnemyStates enemyStates;
    private NavMeshAgent agent;
    private Animator anim;
    private Collider coll;

    //Grunt and Golem inherit and need this value
    protected CharacterStats characterStats;

    [Header("Basic Settings")]
    public float sightRadius;   //how far the enemy can see the player
    public bool isGuard;    //set the states is protal or Guard
    protected GameObject attackTarget;
    private float speed;    //this is walk speed. after chase and walk back to base, will reset this speed to original
    public float lookAtTime;    //Daze time
    private float remainLookAtTime;
    private float lastAttackTime;   //attack rate

    [Header("Patrol State")]
    public float patrolRange;
    private Vector3 wayPoint;   //store random Destination point
    private Vector3 guardPos;   //store the original position
    private Quaternion guardRotation;   //store the original rotation
    //Animator Bool
    bool isWalk;
    bool isChase;
    bool isFollow;
    bool isDead;
    bool isBoardcastStates;

    bool playerDead; //store the player dead state, default is false.

    public bool IsDead { get => isDead; }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        characterStats = GetComponent<CharacterStats>();
        coll = GetComponent<Collider>();

        speed = agent.speed;
        guardPos = transform.position;
        guardRotation = transform.rotation;
        remainLookAtTime = lookAtTime;
        playerDead = false;
        isBoardcastStates = false;
    }

    private void Start()
    {
        if (isGuard)
        {
            enemyStates = EnemyStates.GUARD;
        }
        else {
            enemyStates = EnemyStates.PATROL;
            GetNewWayPoint();
        }
        //TODO: scene switch, this will need to modify
        GameManager.Instance.AddObserver(this);


    }


    //private void OnEnable()
    //{
    //    GameManager.Instance.AddObserver(this);
    //}

    private void OnDisable()
    {
        if (!GameManager.IsInitialized) return;
        GameManager.Instance.RemoveObserver(this);

        if (GetComponent<LootSpawner>() && IsDead) {
            GetComponent<LootSpawner>().Spawnloot();
        }

        if (QuestManager.IsInitialized && isDead) {
            string name = "";
            if (this.name.Contains("Slime"))
            {
                name = "Slime";
            }
            else if (this.name.Contains("TurtleShell")) {
                name = "TurtleShell";
            }

            QuestManager.Instance.UpdateQuestProgress(name, 1);
        }
    }

    private void Update()
    {
        if (characterStats.CurrentHealth == 0) {
            isDead = true;
            if (!isBoardcastStates)
            {
                if (gameObject.name.Contains("Slime")) {
                    AudioManager.Instance.PlaySfx("SlimerDie");
                }
                else if (gameObject.name.Contains("Turtle"))
                {
                    AudioManager.Instance.PlaySfx("TurtleDie");
                }
                else if (gameObject.name.Contains("Grunt"))
                {
                    AudioManager.Instance.PlaySfx("GruntDie");
                }
                else if (gameObject.name.Contains("Golem"))
                {
                    AudioManager.Instance.PlaySfx("GolemDie");
                }
                isBoardcastStates = true;
                RecordStates(gameObject.name);
            }
        }

        //when player is not dead, all actions work
        if (!playerDead)
        {
            SwitchStates();
            SwitchAnimation();
            lastAttackTime -= Time.deltaTime;
        }
    }

    void SwitchAnimation() {
        anim.SetBool("Walk", isWalk);
        anim.SetBool("Chase", isChase);
        anim.SetBool("Follow", isFollow);
        anim.SetBool("Critical", characterStats.isCritical);
        anim.SetBool("Death", IsDead);
    }

    void SwitchStates() {

        if (IsDead)
        {
            enemyStates = EnemyStates.DEAD;
            GetComponent<Collider>().enabled = false;
        }
        else if (FoundPlayer())
        {        //If find player, switch to CHASE
            enemyStates = EnemyStates.CHASE;
        }

        switch (enemyStates)
        {
            case EnemyStates.GUARD:
                isChase = false;
                //if (transform.position != guardPos)
                //{
                if (Vector3.Distance(transform.position, guardPos) > 0.01f)
                {
                    isWalk = true;

                    agent.speed = speed * 0.5f;


                    //Debug.Log("isWalk is true");
                    agent.isStopped = false;
                    agent.destination = guardPos;

                    if (Vector3.SqrMagnitude(guardPos - transform.position) <= agent.stoppingDistance)
                    { 
                        isWalk = false;
                        //Debug.Log("isWalk is false");
                        transform.rotation = Quaternion.Lerp(transform.rotation, guardRotation, 0.01f);
                    }
                }
                break;
            case EnemyStates.PATROL:
                isChase = false;
                agent.speed = speed * 0.5f;
                if (Vector3.Distance(transform.position, wayPoint) <= agent.stoppingDistance)
                {
                    isWalk = false;
                    if (remainLookAtTime > 0) {
                        remainLookAtTime -= Time.deltaTime;
                    }
                    else
                        GetNewWayPoint();
                }
                else {
                    isWalk = true;
                    agent.destination = wayPoint;
                }
                break;
            case EnemyStates.CHASE:

                //Tanimation
                isWalk = false;
                isChase = true;

                agent.speed = speed;

                //if (!FoundPlayer())
                if (attackTarget==null)
                {
                    isFollow = false;
                    if (remainLookAtTime > 0)
                    {
                        agent.destination = transform.position;
                        remainLookAtTime -= Time.deltaTime;
                    }
                    else if (isGuard)
                    {
                        enemyStates = EnemyStates.GUARD;
                    }
                    else
                        enemyStates = EnemyStates.PATROL;
                }
                else {
                    agent.destination = attackTarget.transform.position;
                    isFollow = true;
                    agent.isStopped = false;
                }

                // Attack when under the Attack Range
                if (TargetInAttackRange() || TargetInSkillRange()) {
                    isFollow = false;
                    agent.isStopped = true;

                    if (lastAttackTime < 0) {
                        //lastAttackTime = characterStats.attackData.coolDown;
                        lastAttackTime = characterStats.CoolDown;

                        //check Critical
                        //characterStats.isCritical = Random.value < characterStats.attackData.criticalChance;
                        characterStats.isCritical = Random.value < characterStats.CriticalChance;
                        //do Attack
                        Attack();
                    }

                }

                break;
            case EnemyStates.DEAD:
                coll.enabled = false;
                //agent.enabled = false;
                agent.radius = 0;
                Destroy(gameObject, 2f);
                break;
            default:
                break;
        }
    }

    void Attack() {
        transform.LookAt(attackTarget.transform);
        if (TargetInAttackRange()) {
            //close attack anim
            anim.SetTrigger("Attack");
        }
        if (TargetInSkillRange()) {
            //skill attack anim
            anim.SetTrigger("Skill");
        }
    }

    bool TargetInAttackRange() {
        if (attackTarget != null)
        {
            //return Vector3.Distance(attackTarget.transform.position, transform.position) <= characterStats.attackData.attackRange;
            return Vector3.Distance(attackTarget.transform.position, transform.position) <= characterStats.AttackRange;
        }
        else
            return false;
    }

    bool TargetInSkillRange() {
        if (attackTarget != null)
        {
            return Vector3.Distance(attackTarget.transform.position, transform.position) <= characterStats.attackData.skillRange;
        }
        else
            return false;
    }

    bool FoundPlayer() {
        var colliders = Physics.OverlapSphere(transform.position, sightRadius);
        foreach (var target in colliders)
        {
            if (target.CompareTag("Player")) {
                attackTarget = target.gameObject;
                return true;
            }
        }
        attackTarget = null;
        return false;
    }



    void GetNewWayPoint() {
        remainLookAtTime = lookAtTime;
        float randomX = Random.Range(-patrolRange, patrolRange);
        float randomZ = Random.Range(-patrolRange, patrolRange);

        Vector3 randomPoint = new Vector3(guardPos.x + randomX, transform.position.y, guardPos.z + randomZ);

        NavMeshHit hit;
        wayPoint = NavMesh.SamplePosition(randomPoint, out hit, patrolRange, 1) ? hit.position : transform.position;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, sightRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(wayPoint, 0.5f);
    }

    //Animation Event
    void Hit()
    {
        if (attackTarget != null && transform.IsFacingTarget(attackTarget.transform))
        {
            var targetStats = attackTarget.GetComponent<CharacterStats>();

            targetStats.TakeDamage(characterStats, targetStats);
        }
    }

    public void EndNotify()
    {
        //Win Anim
        //Stop all movement
        //Stop agent
        anim.SetBool("Win", true);
        isChase = false;
        isWalk = false;
        attackTarget = null;
        playerDead = true;
    }


    private void RecordStates(string name)
    {
        switch (name)
        {
            case "Slime1":
                LevelManager.Instance.isDeadSlime1 = true;
                break;
            case "Slime2":
                LevelManager.Instance.isDeadSlime2 = true;
                break;
            case "Slime3":
                LevelManager.Instance.isDeadSlime3 = true;
                break;
            case "TurtleShell1":
                LevelManager.Instance.isDeadTurtle1 = true;
                break;
            case "TurtleShell2":
                LevelManager.Instance.isDeadTurtle2 = true;
                break;
            case "TurtleShell3":
                LevelManager.Instance.isDeadTurtle3 = true;
                break;
            case "Grunt1":
                LevelManager.Instance.isDeadGrunt1 = true;
                break;
            case "Grunt2":
                LevelManager.Instance.isDeadGrunt2 = true;
                break;
            case "Grunt3":
                LevelManager.Instance.isDeadGrunt3 = true;
                break;
            case "Golem1":
                LevelManager.Instance.isDeadGolem1 = true;
                break;
            default:
                break;
        }
    }
}
