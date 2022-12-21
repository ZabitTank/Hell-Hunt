using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BaseItem : MonoBehaviour
{
    public Item item;
    private BoxCollider2D BoxCollider2D;

    private void Awake()
    {
        BoxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        StartCoroutine(ActiveCollider());
    }
    IEnumerator ActiveCollider()
    {
        BoxCollider2D.enabled = false;
        yield return new WaitForSeconds(3.0f);
        BoxCollider2D.enabled = true;

    }
}
