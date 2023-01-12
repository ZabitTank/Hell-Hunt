using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class AIDetector : MonoBehaviour
{
    [SerializeField]
    private float viewRadius = 11f;

    [SerializeField]
    private float detectionDelay = 0.2f;

    [SerializeField]
    private LayerMask playerLayerMask;

    [SerializeField]
    private LayerMask visibilityLayers;

    [SerializeField]
    private Transform target = null;

    public Transform Target
    {
        get
        {
            return target;
        }
        private set
        {
            target = value;
            TargetVisible = false;
        }
    }

    [field: SerializeField]
    public bool TargetVisible { get; private set; }

    private void Start()
    {
        StartCoroutine(PerformDetection());
    }

    private void Update()
    {
        if (Target)
            TargetVisible = CheckTargetVisible();
    }

    private bool CheckTargetVisible()
    {
        var result = Physics2D.Raycast(transform.position,target.position - transform.position,viewRadius,visibilityLayers);
        if(result.collider)
        {
            return (playerLayerMask & (1 << result.collider.gameObject.layer)) != 0;
        }

        return false;
    }

    private void DetectTarget()
    {
        if (!Target)
            CheckIfPlayerInRange();
        else
            CheckIfPlayerOutOfRange();
    }

    private void CheckIfPlayerInRange()
    {
        Collider2D collision = Physics2D.OverlapCircle(transform.position, viewRadius, playerLayerMask);
        if (collision)
            target = collision.transform;
    }

    private void CheckIfPlayerOutOfRange()
    {
        if (!Target || Target.gameObject.activeSelf == false || Vector2.Distance(transform.position, target.position) > viewRadius)
            Target = null;
    }

    IEnumerator PerformDetection()
    {
        yield return new WaitForSeconds(detectionDelay);
        DetectTarget();
        StartCoroutine(PerformDetection());
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, viewRadius);
    }
}
