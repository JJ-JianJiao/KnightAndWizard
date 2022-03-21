using System;
using UnityEngine;
using UnityEngine.AI;

public class BigBrother : MonoBehaviour
{

    private static BigBrother instance;

    private Animator anim;

    private bool isDead;
    private bool isRecover;
    private bool isWalk;
    private bool isLayingDown = true;

    private NavMeshAgent agent;
    public NavMeshObstacle navMO;

    private GameObject followTarget;

    private DialogueController dialogue;

    private Transform destinationPoint;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        navMO = GetComponent<NavMeshObstacle>();
        agent.enabled = false;
        dialogue = GetComponent<DialogueController>();

        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = (BigBrother)this;
        }
        DontDestroyOnLoad(this.gameObject);
        isDead = true;
    }

    private void Start()
    {
        if (LevelManager.Instance) {
            if (LevelManager.Instance.isBigBrotherSaved) {
                gameObject.SetActive(false);
            }
        
        }
    }

    internal void AtDestinationPoint(Transform dest)
    {
        destinationPoint = dest;
        followTarget = null;
    }



    // Update is called once per frame
    void Update()
    {
        if (dialogue.canTalk && isLayingDown) {
            isDead = false;
            isRecover = true;
            isLayingDown = false;
        }

        if (agent.enabled)
        {

            if (followTarget != null && destinationPoint == null)
            {
                agent.stoppingDistance = 4;
                agent.SetDestination(followTarget.transform.position);
                isWalk = true;
                if (Vector3.Distance(transform.position, followTarget.transform.position) <= 4)
                {
                    isWalk = false;
                }
            }

            if (followTarget == null && destinationPoint != null) {
                agent.stoppingDistance = 1;
                agent.SetDestination(destinationPoint.position);
                isWalk = true;
                if (Vector3.Distance(transform.position, destinationPoint.position) <= 1)
                {
                    isWalk = false;
                    agent.enabled = false;
                    Destroy(gameObject,1);
                }
            }

        }
        Animation();
    }

    private void Animation()
    {
        anim.SetBool("Die", isDead);
        anim.SetBool("Recover", isRecover);
        anim.SetBool("Walk", isWalk);
    }

    public void JoinTeam()
    {
        followTarget = GameManager.Instance.playerStates.gameObject;
        navMO.enabled = false;
        agent.enabled = true;
    }
}
