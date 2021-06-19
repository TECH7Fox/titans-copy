using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : Entity
{

    public GameObject prefeb;

    public ResourceType resource;
    public int cost;
    public LayerMask layer;

    private bool built = false;
    private bool spawning = false;
    private float elapsed = 0f;

    private Vector3 TargetPosition;
    private float TargetRadius;
    private int colliders;
    
    // Start is called before the first frame update
    public void Built(Player player)
    {
        this.player = player;
        built = true;
    }
    // FOR TURRETS, RAYCAST FIRST FOR LINE OF SIGHT.

    // ADD TIMING FUNCTION SO IF THERE IS ENOUGH RESOURCES, TAKE IT, AND HAVE A PROCESS BAR DISPLAYING INCREASING.

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (built) // HAVE PROCESS BARS IN UNIVERSAL ENTITY CLASS
        {
            if (spawning == false && player.Wood >= 10)
            {
                player.Wood -= 10;
                spawning = true;
            }
        }

        if (spawning) // LOOK AT HOW TO MAKE YOUR GUI LOOK GOOD, TO MAKE IT NOT SO SHITTY
            SpawnTimer(); // HAVE CIRCEL UI (pi UI) AS MENU
    }

    private void SpawnTimer()
    {
        elapsed += Time.deltaTime;
        progress = elapsed * 10;
        if (elapsed >= 10f)
        {
            progress = 0f;
            elapsed = 0f;
            SpawnUnit();
            spawning = false;
        }
    }

    private void SpawnUnit()
    {
        GameObject entity = Instantiate(prefeb);
        entity.transform.position = this.transform.position + transform.up * 1;
        entity.GetComponent<Unit>().player = player;
        entity.GetComponent<Unit>().setTargetPosition(TargetPosition, TargetRadius);
    }

    public void setTargetPosition(Vector3 pos, float radius)
    {
        TargetPosition = pos;
        TargetRadius = radius;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("COLLING: TRIGGER: " + other.gameObject.layer);
        if (other.gameObject.layer != layer)
        {
            colliders += 1;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != layer)
        {
            colliders -= 1;
        }
    }

    public bool IsValidPlace()
    {
        if (colliders <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
