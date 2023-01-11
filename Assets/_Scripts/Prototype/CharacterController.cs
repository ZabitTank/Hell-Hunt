using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public GameObject character;

    public Animator feetAnimator;
    public Animator bodyAnimator;

    public bool isMoving = false;
    public Vector2 movingDirection = Vector2.zero;

    public bool isRotate = false;
    public Vector2 rotateDirection = Vector2.zero;
    private Vector2 lookDirection = Vector2.zero;

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

    public void HandleState(float horizontalInput,float verticalInput,Vector2 newRotateDirection)
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

}
