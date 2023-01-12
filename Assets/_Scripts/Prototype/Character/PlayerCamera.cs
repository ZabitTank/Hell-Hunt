using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera Instance { get; private set; }

    [SerializeField]
    [Header("Distance Camera Z axis camera to player")]
    private float height;

    Camera m_OrthographicCamera;
    public Transform playerTranforms;

    private void Awake()
    {
        m_OrthographicCamera = GetComponent<Camera>();
    }

    private void Start()
    {
        playerTranforms = GlobalVariable.Instance.playerReferences.playerTransform;
        m_OrthographicCamera.enabled = true;
        if (m_OrthographicCamera)
        {
            m_OrthographicCamera.orthographic = true;
        }
    }
    private void LateUpdate()
    {
        transform.position = new(playerTranforms.position.x, playerTranforms.position.y,-10);
    }

    public void ChangeHeight(float height)
    {
        Instance.m_OrthographicCamera.orthographicSize = height;
    }
}
