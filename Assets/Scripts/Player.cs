using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int Wood;
    public int Stone;
    public int Metal;
    public Color color;

    public Text WoodLabel;
    public Text StoneLabel;
    public Text MetalLabel;

    // Update is called once per frame
    void Update()
    {
        WoodLabel.text = "Wood: " + Wood;
        StoneLabel.text = "Stone: " + Stone;
        MetalLabel.text = "Metal: " + Metal;
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
