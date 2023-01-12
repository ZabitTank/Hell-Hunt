using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPatrolPathBehaviour : AIBeheviour
{
    [SerializeField]
    private PatrolPath patrolPath;

    public float patrolSpeed = 1;

    [Range(0.1f, 1)]
    public float arriveDistance = 1;

    public float delayTime = 0.5f;
    public float countDelay = 0;

    [SerializeField]
    private bool isWaiting = false;

    [SerializeField]
    Vector2 currentPatrolTarget = Vector2.zero;
    bool isInitialized = false;

    private int currentIndex = -1;

    private void Awake()
    {
        if(!patrolPath)
            patrolPath = GetComponentInChildren<PatrolPath>();
    }

    //private void Start()
    //{
    //    var currentPathPoint = patrolPath.GetClosestPathPoint(characterController.character.transform.position);
    //    currentIndex = currentPathPoint.index;
    //    currentPatrolTarget = currentPathPoint.position;
    //    isInitialized = true;
    //}
    public override void PerformAction(CharacterController characterController, AIDetector detector)
    {
        if (isWaiting) return;

        if (patrolPath.Length < 2) return;

        if (!isInitialized)
        {
            var currentPathPoint = patrolPath.GetClosestPathPoint(characterController.character.transform.position);
            currentIndex = currentPathPoint.index;
            currentPatrolTarget = currentPathPoint.position;
            isInitialized = true;
        }

        if (Vector2.Distance(characterController.character.transform.position,currentPatrolTarget) < arriveDistance)
        {
            isWaiting = true;
            StartCoroutine(WaitCoroutine());
            return;
        }

        Vector2 direction = currentPatrolTarget - (Vector2)characterController.character.transform.position;
        characterController.HandleStateWithTarget(direction, patrolSpeed, direction, 200f);
    }

    IEnumerator WaitCoroutine()
    {
        yield return new WaitForSeconds(delayTime);

        var nextPathPoint = patrolPath.GetNextPathPoint(currentIndex);

        currentIndex = nextPathPoint.index;
        currentPatrolTarget = nextPathPoint.position;

        isWaiting = false;
    }
}
