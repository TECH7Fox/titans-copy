using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public Player player;
    public GameObject prefeb;

    public ResourceType resource;
    public int cost;

    private bool built = false;
    private bool spawning = false;
    private float elapsed = 0f;

    private Vector3 TargetPosition;
    private float TargetRadius;
    
    // Start is called before the first frame update
    public void Built(Player player)
    {
        this.player = player;
        built = true;
    }


    // ADD TIMING FUNCTION SO IF THERE IS ENOUGH RESOURCES, TAKE IT, AND HAVE A PROCESS BAR DISPLAYING INCREASING.

    // Update is called once per frame
    void Update()
    {
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

        if (GetComponent<selection_component>() != null)
        {
            GetComponent<selection_component>().progress = elapsed / 10;
        }
    }

    private void SpawnTimer()
    {
        elapsed += Time.deltaTime;
        if (elapsed >= 10f)
        {
            elapsed = 0f;
            SpawnUnit();
            spawning = false;
        }
    }

    private void SpawnUnit()
    {
        GameObject entity = Instantiate(prefeb);
        entity.transform.position = this.transform.position + transform.up * 1;
        entity.GetComponent<EntityController>().player = player;
        entity.GetComponent<EntityController>().setTargetPosition(TargetPosition, TargetRadius);
    }

    public void setTargetPosition(Vector3 pos, float radius)
    {
        TargetPosition = pos;
        TargetRadius = radius;
    }
}
