using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIShootingBehaviour : AIBeheviour
{
    public float shootingRange = 60;
    public bool targetInRange = false;

    public override void PerformAction(CharacterController characterController, AIDetector detector)
    {
        if (TargetInRange(characterController, detector))
        {
            characterController.PerformShootAnimation();
        } 
    }

    private bool TargetInRange(CharacterController characterController,AIDetector detector)
    {
        targetInRange = false;
        var targetDirection = detector.Target.position - characterController.character.transform.position;
        if(Vector2.Angle(characterController.transform.right,targetDirection) < shootingRange / 2)
        {
            targetInRange = true;
        }
        return targetInRange;
    }
}
