using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BaseEnemyAI : MonoBehaviour
{
    [SerializeField]
    AIBeheviour attackBehaviour, patrolBehaviour;

    [SerializeField]
    CharacterController characterController;

    [SerializeField]
    AIDetector detector;

    private void Awake()
    {
        detector = GetComponentInChildren<AIDetector>();
        characterController = GetComponentInChildren<CharacterController>();
        characterController.setParent(gameObject);
    }

    private void Update()
    {
        if(detector.TargetVisible)
        {
            attackBehaviour.PerformAction(characterController, detector);
        }
        else
        {
            patrolBehaviour.PerformAction(characterController, detector);
        }
    }
}
