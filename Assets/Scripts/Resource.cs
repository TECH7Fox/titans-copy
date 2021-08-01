using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ResourceType
{
    Wood,
    Stone,
    Metal
}

public class Resource : MonoBehaviour
{
    public ResourceType type;
    public int quantity;

    public void GatherResource(int amount, Player player)
    {
        quantity -= amount;
        player.AddResource(type, amount);

        if (quantity <= 0)
            GetComponent<Animator>().SetBool("Fall", true);
    }

    public void DestroyResource()
    {
        Destroy(gameObject);
    }
}
