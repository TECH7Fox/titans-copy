using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseLight : MonoBehaviour
{
    public Transform cursorLight;
    public float distance = 500f;
    public LayerMask layerMask;
    public float distanceFromHit = 10;
    public Transform planet;

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, distance, layerMask))
        {
            cursorLight.position = hitInfo.point + (hitInfo.point - planet.position).normalized * distanceFromHit;            
        } else
        {
            cursorLight.position = Vector3.zero;
        }
    }
}
