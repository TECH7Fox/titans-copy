using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class Woodchopper : Entity
{

    Vector3 TargetPosition;
    Vector3 LookAtTarget;
    Quaternion PlayerRotation;

    private float elapsed = 0f;
    private float TargetRadius;

    public float RotateSpeed = 5;
    public float Speed = 10;

    private bool Moving = false;
    private bool gathering = false;
    public GameObject resource;
    public Building building;

    public float seeDistance = 50;

    protected override void Update()
    {
        base.Update();

        Debug.Log("update");
        if (resource == null)
        {
            Debug.Log("something");
            GameObject tree = FindNearestObject("Tree");
            if (tree != null)
            {
                Debug.Log("setting stuff");
                SetResourceTarget(tree);
            }
        }

        if (Moving)
            Move();

        if (gathering)
            Gather();
    }

    private GameObject FindNearestObject(string objectName)
    {
        GameObject tMin = null;
        Vector3 currentPos = transform.position;
        GameObject[] objects = GameObject.FindGameObjectsWithTag(objectName);

        if (objects.Length == 0) return null;

        foreach (GameObject gameObject in objects)
        {
            float dist = Vector3.Distance(gameObject.transform.position, currentPos);
            {
                tMin = gameObject;
            }
        }
        return tMin;
    }

    public void SetTargetPosition(Vector3 pos, float radius)
    {
        TargetPosition = pos;
        TargetRadius = radius;
        LookAtTarget = new Vector3(TargetPosition.x - transform.position.x,
            transform.position.y,
            TargetPosition.z - transform.position.z);
        PlayerRotation = Quaternion.LookRotation(LookAtTarget);
        Moving = true;
    }

    public void SetResourceTarget(GameObject selectedObject)
    {
        resource = selectedObject;
        resource.GetComponent<Outline>().enabled = true;
        SetTargetPosition(resource.transform.position, 3);
    }

    public void SetResourceNull()
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
                resource.GetComponent<Animator>().SetTrigger("Hit");
                resource.GetComponent<Resource>().GatherResource(1, player);
            }
            else if (!resource)
            {
                gathering = false;
                //move back to base
            }
        }
    }
}