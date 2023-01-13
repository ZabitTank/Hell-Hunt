using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Animations;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public GameObject character;

    public Animator feetAnimator;
    public Animator bodyAnimator;
    public Animator muzzleAnimator;
    public Transform muzzlePosition;

    private bool isMoving = false;
    public Vector2 movingDirection = Vector2.zero;

    public bool isRotate = false;
    private Vector2 rotateDirection = Vector2.zero;

    public Vector2 currentTargetPosition;

    float rotateSpeed = 0.1f;
    float movementSpeed = 1f;
    public void setParent(GameObject _character)
    {
        character = _character;
    }

    public void PerformMovingAnimation()
    {
        feetAnimator.SetBool("Move", isMoving);
        bodyAnimator.SetBool("Move", isMoving);
    }

    public void PerformMeleeAttackAniamtion()
    {
        bodyAnimator.SetTrigger("Melee");
    }
    public void PerformShootAnimation()
    {
        bodyAnimator.SetTrigger("Shoot");
        muzzleAnimator.SetTrigger("Shoot");
    }

    public void PerformReload()
    {
        bodyAnimator.Play("Reload");
    }

    public void SetCharacterRotation()
    {
        if (isRotate)
        {
            float rotation = Mathf.Atan2(rotateDirection.y, rotateDirection.x) * Mathf.Rad2Deg;
            character.transform.rotation = Quaternion.Euler(0, 0, rotation);
        };
        if (isMoving)
        {
            float angle = Mathf.Atan2(movingDirection.y, movingDirection.x) * Mathf.Rad2Deg;
            feetAnimator.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            if (feetAnimator.transform.localEulerAngles.z > 90 && feetAnimator.transform.localEulerAngles.z < 270)
            {
                feetAnimator.transform.localScale = new(-1, -1, -1);
            }
            else
            {
                feetAnimator.transform.localScale = new(1, 1, 1);
            }
        };
    }

    public void CharacterRotate(Vector2 movingDirection,Vector2 targetPosition)
    {
        var targetDirection = (Vector3)targetPosition - muzzlePosition.position;

        var angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;

        var rotateStep = rotateSpeed * Time.deltaTime;

        character.transform.rotation = Quaternion.RotateTowards(character.transform.rotation, Quaternion.Euler(0, 0, angle)
            , rotateStep);

        if (isMoving)
        {
            movingDirection = movingDirection.normalized;

            character.transform.Translate(movingDirection * movementSpeed * Time.deltaTime,Space.World);

            float feetAngle = Mathf.Atan2(movingDirection.y, movingDirection.x) * Mathf.Rad2Deg;
            feetAnimator.transform.rotation = Quaternion.AngleAxis(feetAngle, Vector3.forward);
            if (feetAnimator.transform.localEulerAngles.z > 90 && feetAnimator.transform.localEulerAngles.z < 270)
            {
                feetAnimator.transform.localScale = new(-1, -1, -1);
            }
            else
            {
                feetAnimator.transform.localScale = new(1, 1, 1);
            }
        };
    }

    public void HandleStateWithMouse(float horizontalInput, float verticalInput, Vector2 newRotateDirection)
    {
        movingDirection = new(horizontalInput, verticalInput);
        isMoving = movingDirection.magnitude > 0.00f;

        if (rotateDirection != newRotateDirection)
        {
            rotateDirection = newRotateDirection;
            isRotate = true;
        }
        PerformMovingAnimation();
        SetCharacterRotation();
    }

    public void HandleStateWithTarget(Vector2 movingPosition,float _movementSpeed, Vector2 newTargetPosition, float newRotateSpeed)
    {
        movementSpeed = _movementSpeed;
        isMoving = movingPosition.magnitude > 0.00f;

        rotateSpeed = newRotateSpeed;

        CharacterRotate(movingPosition - (Vector2)character.transform.position,newTargetPosition);
    }

}
