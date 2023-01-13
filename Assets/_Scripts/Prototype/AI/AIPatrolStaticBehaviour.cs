using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AIPatrolStaticBehaviour : AIBeheviour
{
    public float patrolDelay = 1.5f;
    public float rotateSpeed = 1f;

    [SerializeField]
    public Vector2 randomDirection = Vector2.zero;
    [SerializeField]
    private float countDelay = 0f;
    private void Start()
    {
        randomDirection = (Vector2) Parent.transform.position +  Random.insideUnitCircle *10;
    }
    public override void PerformAction(CharacterController characterController, AIDetector detector)
    {
        if (countDelay <= 0)
        {
            randomDirection = (Vector2)Parent.transform.position + Random.insideUnitCircle * 10;
            countDelay = patrolDelay;
        } else
        {
            if (countDelay > 0)
                countDelay -= Time.deltaTime;
            characterController.HandleStateWithTarget(Vector2.zero,0, randomDirection,rotateSpeed);
            
        }
    }
}
