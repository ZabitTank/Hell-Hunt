using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float force;
    public int damage;
    public Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void InitState(float speed, int damage, float spread)
    {
        this.force = speed;
        this.damage = damage;
        transform.Rotate(Vector3.forward, spread);
    }

    public int timer = 1;
    void Start()
    {
        rb.AddForce(transform.right * force * 50);
        Destroy(gameObject, 2);
    }

    //void Update()
    //{
    //    transform.position += transform.right * Time.deltaTime * force;
    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var obj = collision.gameObject;
        if(obj.CompareTag("Player"))
        {
            var enemy = obj.GetComponent<Player>();
            enemy.TakeDamge(-damage);
        } else if (obj.CompareTag("Enemy"))
        {
            var enemy = obj.GetComponent<BaseEnemyAI>();
            enemy.TakeDamage(-damage);
        }
        Destroy(gameObject);
    }
}
