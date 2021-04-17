using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FauxGravityAttractor : MonoBehaviour
{
    public float Gravity = -10;

    public void Attract(Transform body)
    {
        Vector3 GravityUp = (body.position - transform.position).normalized;
        Vector3 BodyUp = body.up;

        body.GetComponent<Rigidbody>().AddForce(GravityUp * Gravity);

        Quaternion targetRotation = Quaternion.FromToRotation(BodyUp, GravityUp) * body.rotation;
        body.rotation = Quaternion.Slerp(body.rotation, targetRotation, 50 * Time.deltaTime);
    }
}
