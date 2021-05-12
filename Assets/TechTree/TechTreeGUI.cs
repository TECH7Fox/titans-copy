using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class TechTreeGUI : MonoBehaviour
{
    public TechTreeNodeGraph techTree;
    public GameObject nodePrefeb;
    public GameObject tierPrefeb;

    /*private Dictionary<int, GameObject> tier = new(); //?

    private void Start()
    {
        foreach (TechTreeNode node in techTree.nodes)
        {
            if (GameObject.Find("tier " + node.tier) == null)
            {
                Debug.Log(tierPrefeb);
                tier.Add(node.tier, Instantiate(tierPrefeb));
                tier[node.tier].name = "tier " + node.tier;
                tier[node.tier].transform.SetParent(transform);
            }
        GameObject newNode = Instantiate(nodePrefeb);
        newNode.transform.SetParent(tier[node.tier].transform);
        }

        foreach (TechTreeNode node in techTree.nodes)
        {
            if (node.childeren.Length > 0)
            {

            }
        }
    }*/
}
