using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public Player player;
    public float health;
    public float progress;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (health <= 0)
        {
            Debug.Log("destroy");
            FindObjectOfType<selected_dictionary>().deselect(gameObject.GetInstanceID());
            Destroy(gameObject);
        }
    }
}
