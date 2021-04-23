using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Player player;
    public float speed = 5f;
    public float lifespan = 3f;
    public int damage = 1;

    public void Init(Player player, float speed, float lifespan, int damage)
    {
        this.player = player;
        this.speed = speed;
        this.lifespan = lifespan;
        this.damage = damage;
    }

    // Start is called before the first frame update
    private void Start()
    {
        GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * speed);
        Destroy(gameObject, lifespan);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Entity"))
        {
            if (collider.gameObject.GetComponent<Entity>().player == player)
            {
                return;
            }
            collider.gameObject.GetComponent<Entity>().health -= 10;
        }
        Destroy(gameObject);
    }
}
