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
            float angle;
            Quaternion randomDir = Quaternion.Euler(Random.Range(-10, 10), Random.Range(-10, 10), 0);
            //Vector3 extraHeight = target.transform.up * (distance / 20);
            //Debug.Log(target.transform.position + extraHeight);                       // PLEASE DO NOT LOOK AT THIS PART. THANK YOU
            CalculateTrajectory(transform.position, target.transform.position, bulletSpeed, out angle);
            bulletTransform = Instantiate(bullet, transform.position, randomDir * Quaternion.LookRotation(target.transform.position - transform.position));
            bulletTransform.GetComponent<Bullet>().Init(player, bulletSpeed, 10f, 1);
        }
    }

    public static bool CalculateTrajectory(Vector3 start, Vector3 end, float muzzleVelocity, out float angle)
    {//, out float highAngle){

        Vector3 dir = end - start;
        float vSqr = muzzleVelocity * muzzleVelocity;
        float y = dir.y;
        dir.y = 0.0f;
        float x = dir.sqrMagnitude;
        float g = -Physics.gravity.y;

        float uRoot = vSqr * vSqr - g * (g * (x) + (2.0f * y * vSqr));


        if (uRoot < 0.0f)
        {

            //target out of range.
            angle = -45.0f;
            //highAngle = -45.0f;
            return false;
        }

                float r = Mathf.Sqrt (uRoot);
                float bottom = g * Mathf.Sqrt (x);

        //angle = -Mathf.Atan2(g * Mathf.Sqrt(x), vSqr + Mathf.Sqrt(uRoot)) * Mathf.Rad2Deg;
        angle = -Mathf.Atan2 (bottom, vSqr - r) * Mathf.Rad2Deg;
        return true;

    }
}