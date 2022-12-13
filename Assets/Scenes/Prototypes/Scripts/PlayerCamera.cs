using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField]
    [Header("Distance Camera Z axis camera to player")]
    private float height;

    public Transform playerTranforms;
    private void LateUpdate()
    {
        transform.position = playerTranforms.position - Vector3.forward * height;
    }
}
