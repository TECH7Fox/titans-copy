using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5f;
    public float lifespan = 3f;
    public int damage = 1;
    private Rigidbody rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        rigidbody.AddForce(gameObject.transform.forward * speed);
        Destroy(gameObject, 5f);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Entity"))
        {
            collider.gameObject.GetComponent<Entity>().health -= 10;
            Debug.Log("HIT ENTITY");
        }
        Destroy(gameObject);
    }
}
