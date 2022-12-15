using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 10;
    public int speed = 50;

    public int timer = 1;
    void Start()
    {
        Destroy(gameObject, timer);
    }

    void Update()
    {
        transform.position += transform.right * Time.deltaTime * speed;
    }
}
