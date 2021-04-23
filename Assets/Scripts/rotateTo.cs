using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateTo : MonoBehaviour
{
    public Transform Objtransform;

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Objtransform.position);
    }
}
