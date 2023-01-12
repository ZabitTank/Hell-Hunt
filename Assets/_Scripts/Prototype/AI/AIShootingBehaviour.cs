using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIShootingBehaviour : AIBeheviour
{
    public float shootingPov = 60;
    public bool targetInRange = false;

    public float rotateSpeed = 200;

    public override void PerformAction(CharacterController characterController, AIDetector detector)
    {
        characterController.HandleStateWithTarget(0, 0, detector.Target.position, rotateSpeed);
        if (TargetInRange(characterController, detector))
        {
            Parent.weapon.DoPrimaryAttack();
        }
    }

    private bool TargetInRange(CharacterController characterController,AIDetector detector)
    {
        targetInRange = false;
        var targetDirection = detector.Target.position - characterController.muzzlePosition.right;
        if( Vector2.Angle(characterController.muzzlePosition.right,targetDirection) < shootingPov / 2)
        {
            targetInRange = true;
        }
        return targetInRange;
    }
}
