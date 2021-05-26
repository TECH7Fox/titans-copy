using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Dictionary<string, int> unlocked = new Dictionary<string, int>();
    public string currentResearch;
    
    public int Wood;
    public int Stone;
    public int Metal;
    public int research = 100;

    public Color color;

    public Text WoodLabel;
    public Text StoneLabel;
    public Text MetalLabel;
    //public Text researchLabel;

    private void Awake()
    {
        unlocked.Add("tower", 0);
        unlocked.Add("factory", 0);
        unlocked.Add("cannon", 0);
    }

    // Update is called once per frame
    private void Update()
    {
        //research += 1; //test

        WoodLabel.text = "Wood: " + Wood;
        StoneLabel.text = "Stone: " + Stone;
        MetalLabel.text = "Metal: " + Metal;

        if (!string.IsNullOrEmpty(currentResearch))
        {
            if (unlocked[currentResearch] < 100 && research > 0)
            {
                unlocked[currentResearch] += 1;
                research -= 1;
            }
        }
    }

    public void AddResource(ResourceType resource, int amount)
    {
        switch (resource.ToString())
        {
            case "Wood": Wood += amount; break;
            case "Stone": Stone += amount; break;
            case "Metal": Metal += amount; break;
        }
    }

    public void RemoveResource(ResourceType resource, int amount)
    {
        switch (resource.ToString())
        {
            case "Wood": Wood -= amount; break;
            case "Stone": Stone -= amount; break;
            case "Metal": Metal -= amount; break;
        }
    }

    public bool HasResource(ResourceType resource, int amount)
    {
        switch (resource.ToString())
        {
            case "Wood": return (Wood >= amount);
            case "Stone": return (Stone >= amount);
            case "Metal": return (Metal >= amount);
            default: return false;
        }
    }
}
