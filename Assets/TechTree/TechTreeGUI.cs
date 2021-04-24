using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class TechTreeGUI : MonoBehaviour
{
    public TechTreeNodeGraph techTree;
    public GameObject nodePrefeb;
    public GameObject tierPrefeb;

    private Dictionary<int, GameObject> tier = new Dictionary<int, GameObject>();

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
            //newNode.transform.Find("Title").GetComponent<TMPro.TextMeshProUGUI>().text = node.title;
/*            for (int i = 0; i <= node.childeren.Length; i++)
            {
                //ResearchButton newNode = Instantiate(nodePrefeb).AddComponent<ResearchButton>();
                //newNode.Init(node);
                //newNode.transform.parent = transform;
                //newNode.transform.position = new Vector3(100, 100, 0);
                // set node pos based on i
            }*/
        }
    }
}
