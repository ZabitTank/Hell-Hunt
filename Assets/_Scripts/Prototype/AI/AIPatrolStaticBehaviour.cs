using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPatrolStaticBehaviour : AIBeheviour
{
    public float patrolDelay = 2f;

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
        float angle = Vector2.Angle(characterController.character.transform.right, randomDirection);
        if(countDelay <= 0 && (angle < 2))
        {
            randomDirection = Random.insideUnitCircle;
            countDelay = patrolDelay;
        } else
        {
            if(countDelay > 0)
                countDelay -= Time.deltaTime;
            else
            {
                characterController.HandleState(0, 0, randomDirection);
            }
        }
    }
}
