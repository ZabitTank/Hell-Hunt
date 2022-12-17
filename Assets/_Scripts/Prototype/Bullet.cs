using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public int damage;
    public Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void InitState(float speed, int damage, float spread)
    {
        this.speed = speed;
        this.damage = damage;
        transform.Rotate(Vector3.forward, spread);
    }

    public int timer = 1;
    void Start()
    {
        Destroy(gameObject, timer);
    }

    void Update()
    {
        transform.position += transform.right * Time.deltaTime * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
}
