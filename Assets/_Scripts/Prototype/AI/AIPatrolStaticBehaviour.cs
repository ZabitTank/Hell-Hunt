using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPatrolStaticBehaviour : AIBeheviour
{
    public float patrolDelay = 1.5f;
    public float rotateSpeed = 1f;

    [SerializeField]
    public Vector2 randomDirection = Vector2.zero;
    [SerializeField]
    private float countDelay = 0f;


    private void Awake()
    {
        randomDirection = Random.insideUnitCircle;
    }
    public override void PerformAction(CharacterController characterController, AIDetector detector)
    {
        if(countDelay <= 0 && Vector2.Angle(characterController.muzzlePosition.right, randomDirection) < 2f)
        {
            randomDirection = Random.insideUnitCircle;
            countDelay = patrolDelay;
        } else
        {
            if (countDelay > 0)
                countDelay -= Time.deltaTime;
            else
                characterController.HandleStateWithTarget(0, 0, randomDirection,rotateSpeed);
            
        }
    }
}
