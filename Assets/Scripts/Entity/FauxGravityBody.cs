using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FauxGravityBody : MonoBehaviour
{
    private FauxGravityAttractor attractor;
    private Transform MyTransform;

    // Start is called before the first frame update
    void Start()
    {
        attractor = FindClosestAttractor().GetComponent<FauxGravityAttractor>();
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        GetComponent<Rigidbody>().useGravity = false;
        MyTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        attractor.Attract(MyTransform);
    }

    private GameObject FindClosestAttractor()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Attractor");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }
}
