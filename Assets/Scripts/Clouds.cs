using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud
{
    public GameObject cloud;
    public Vector2 direction;
    public float speed;

    public Cloud(GameObject cloud, Vector2 direction, float speed)
    {
        this.cloud = cloud;
        this.direction = direction;
        this.speed = speed;
    }
}

public class Clouds : MonoBehaviour
{
    public int amount;
    public float speed;
    public float height;

    public GameObject[] cloudPrefebs = new GameObject[5];

    private List<Cloud> clouds = new List<Cloud>();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < amount; i++)
        {
            clouds.Add(new Cloud(Instantiate(cloudPrefebs[Random.Range(0, 5)], Random.onUnitSphere * Random.Range(height, height + 15), Quaternion.identity), Random.insideUnitCircle.normalized, Random.Range(5f, 15f)));
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Cloud cloud in clouds)
        {
            Transform cloudTransform = cloud.cloud.transform;
            cloudTransform.RotateAround(this.transform.position, cloud.direction, cloud.speed * Time.deltaTime);
            Vector3 GravityUp = (cloudTransform.position - transform.position).normalized;
            Vector3 BodyUp = cloudTransform.up;
            Quaternion targetRotation = Quaternion.FromToRotation(BodyUp, GravityUp) * cloudTransform.rotation;
            cloudTransform.rotation = Quaternion.Slerp(cloudTransform.rotation, targetRotation, 50 * Time.deltaTime);
        }
    }
}
