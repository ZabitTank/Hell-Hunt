using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AutoDestroyObject : MonoBehaviour
{
    public float timeToDestroy;
    private void Start()
    {
        Destroy(gameObject, timeToDestroy);
    }
}
