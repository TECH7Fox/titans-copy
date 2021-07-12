using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class Unit : Entity
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

    public float seeDistance = 50;
    public GameObject bullet;

    private GameObject bulletTransform;
    private float distance;

    protected override void Update()
    {
        base.Update();
        if (checkForEnemy() != null)
        {
            // maby replace with Bullet.CreateInstance("name") as Bullet; or ScriptableObjet.CreateInstance<Bullet>();
            Shoot();
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
        Entity[] gos;
        gos = FindObjectsOfType<Entity>();
        GameObject closest = null;
        distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (Entity go in gos)
        {
            if (go.player != player)
            {
                Vector3 diff = go.transform.position - position;
                float curDistance = diff.sqrMagnitude;
                if (curDistance <= seeDistance && curDistance < distance)
                {
                    closest = go.gameObject;
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
                resource.GetComponent<Animator>().SetTrigger("Hit");
                resource.GetComponent<Resource>().GatherResource(1, player);
            }
            else if (!resource)
            {
                gathering = false;
            }
        }
    }

    void Shoot()
    {
        elapsed += Time.deltaTime;
        progress = elapsed * 100;
        if (elapsed >= 1f)
        {
            progress = 0;
            elapsed = 0f;
            GameObject target = checkForEnemy();
            float bulletSpeed = 500f;
            Quaternion randomDir = Quaternion.Euler(90, 0, 0);
            //Vector3 extraHeight = target.transform.up * (distance / 20);
            //Debug.Log(target.transform.position + extraHeight);                       // PLEASE DO NOT LOOK AT THIS PART. THANK YOU
            bulletTransform = Instantiate(bullet, transform.position, Quaternion.LookRotation(target.transform.position - transform.position));
            bulletTransform.GetComponent<Bullet>().Init(player, bulletSpeed, 10f, 1);
            bulletTransform.transform.LookAt(target.transform.position + (transform.up * Vector3.Distance(target.transform.position, transform.position) / 3));
        }
    }

    void CalculateAngleToHitTarget(GameObject target, out float? theta1, out float? theta2)
    {
        //Initial speed
        float v = 10f;

        Vector3 targetVec = target.transform.position - transform.position;

        //Vertical distance
        float y = Vector3.Distance(target.transform.position, new Vector3(0, 0, 0)) - Vector3.Distance(transform.position, new Vector3(0, 0, 0));
        Debug.Log("Y: " + y);

        //Reset y so we can get the horizontal distance x
        targetVec.y = 0f;

        //Horizontal distance
        float x = targetVec.magnitude;
        
        Vector3 spokeToActual = transform.position - new Vector3(0, 0, 0),
         spokeToCorrect = target.transform.position - new Vector3(0, 0, 0);
        float angleFromCenter = Vector3.Angle(spokeToActual, spokeToCorrect);
        // NOTE: angle inputs don't need normalize. In degrees(!!)

        //float x = 2 * Mathf.PI * 53.3f * (angleFromCenter / 360);
        Debug.Log("DISTANCE: " + x);

        //Gravity
        float g = 10f;


        //Calculate the angles

        float vSqr = v * v;

        float underTheRoot = (vSqr * vSqr) - g * (g * x * x + 2 * y * vSqr);

        //Check if we are within range
        if (underTheRoot >= 0f)
        {
            float rightSide = Mathf.Sqrt(underTheRoot);

            float top1 = vSqr + rightSide;
            float top2 = vSqr - rightSide;

            float bottom = g * x;

            theta1 = Mathf.Atan2(top1, bottom) * Mathf.Rad2Deg;
            theta2 = Mathf.Atan2(top2, bottom) * Mathf.Rad2Deg;
        }
        else
        {
            theta1 = null;
            theta2 = null;
        }
    }
}