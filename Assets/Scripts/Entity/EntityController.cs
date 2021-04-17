using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour
{
    Vector3 TargetPosition;
    Vector3 LookAtTarget;
    Quaternion PlayerRotation;
    
    public float RotateSpeed = 5;
    public float Speed = 10;

    private float elapsed = 0f;
    private float TargetRadius;
    
    private bool Moving = false;
    private bool gathering = false;

    public GameObject resource;
    public Player player;

    public float seeDistance = 10;
    public GameObject bullet;

    private GameObject bulletTransform;
    private Vector3 shootDir;

    void Update()
    {
       if (checkForEnemy() != null)
        {
            bulletTransform = Instantiate(bullet, transform.position, Quaternion.LookRotation(checkForEnemy().transform.position - transform.position));
        }

        if (Moving)
            Move();

        if (gathering)
            Gather();

    }

    public void setTargetPosition(Vector3 pos, float radius)
    {
        TargetPosition = pos;
        TargetRadius = radius;
        LookAtTarget = new Vector3(TargetPosition.x - transform.position.x,
            transform.position.y,
            TargetPosition.z - transform.position.z);
        PlayerRotation = Quaternion.LookRotation(LookAtTarget);
        Moving = true;
    }

    public void setResourceTarget(GameObject selectedObject)
    {
        resource = selectedObject;
        resource.GetComponent<Outline>().enabled = true;
        setTargetPosition(resource.transform.position, 3);
    }

    public GameObject checkForEnemy()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Unit");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            if (go.GetComponent<EntityController>().player != player)
            {
                Vector3 diff = go.transform.position - position;
                float curDistance = diff.sqrMagnitude;
                if (curDistance <= seeDistance && curDistance < distance)
                {
                    closest = go;
                    distance = curDistance;
                }
            }
        }
        return closest;
    }

    public void setResourceNull()
    {
        if (resource != null)
            resource.GetComponent<Outline>().enabled = false;
        resource = null;
    }

    void Move()
    {
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            PlayerRotation,
            RotateSpeed * Time.deltaTime);
        transform.position = Vector3.MoveTowards(
            transform.position,
            TargetPosition,
            Speed * Time.deltaTime);

        //Debug.Log(Vector3.Distance(transform.position, TargetPosition) + " | " + TargetRadius);
        if (Vector3.Distance(transform.position, TargetPosition) <= TargetRadius)
        {
            Moving = false;
            if (resource != null)
                gathering = true;
        }
            
    }

    void Gather()
    {
        elapsed += Time.deltaTime;
        if (elapsed >= 1f)
        {
            elapsed = 0f;
            if (resource != null)
            {
                resource.GetComponent<Resource>().GatherResource(1, player);
            }
            else if (!resource)
            {
                gathering = false;
            }
        }
    }
}
