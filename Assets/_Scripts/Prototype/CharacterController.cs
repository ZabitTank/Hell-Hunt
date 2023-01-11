using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    private GameObject character;

    public Animator feetAnimator;
    public Animator bodyAnimator;

    public void setParent(GameObject _character)
    {
        character = _character;
    }

    public void SetMovingAnimation(bool isMoving)
    {
        feetAnimator.SetBool("Move", isMoving);
        bodyAnimator.SetBool("Move", isMoving);
    }

    public void PerformMeleeAttack()
    {
        bodyAnimator.SetTrigger("Melee");
    }
    public void PerformShoot()
    {
        bodyAnimator.SetTrigger("Shoot");
    }

    public void PerformReload()
    {
        bodyAnimator.Play("Reload");
    }

    public void SetCharacterRotation(Vector2 movingDirection, bool isMoving,Vector2 rorateDirection, bool isRorate)
    {
        if (isRorate)
        {
            float rotation = Mathf.Atan2(rorateDirection.y, rorateDirection.x) * Mathf.Rad2Deg;
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


}
