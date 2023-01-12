using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class AIShootingBehaviour : AIBeheviour
{
    public float shootingPov = 60;
    public bool targetInRange = false;

    public float rotateSpeed = 200;


    public override void PerformAction(CharacterController characterController, AIDetector detector)
    {
        characterController.HandleStateWithTarget(Vector2.zero, 0, detector.Target.position, rotateSpeed);
        if (TargetInRange(characterController, detector))
        {
            Parent.weapon.DoPrimaryAttack();
        }
    }

    private bool TargetInRange(CharacterController characterController,AIDetector detector)
    {
        targetInRange = false;
        var targetDirection = detector.Target.position - characterController.muzzlePosition.position;
        if( Vector2.Angle(characterController.muzzlePosition.right,targetDirection) < shootingPov / 2)
        {
            targetInRange = true;
        }
        return targetInRange;
    }

}
