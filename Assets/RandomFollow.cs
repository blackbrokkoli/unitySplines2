using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomFollow : MonoBehaviour
{
    GameObject follow;
    Vector3 followBehind;
    float timePassed = 0f;
    // set an offset to the follow object for all dimensions

    public float timeUntilSwitch = 12f;
    public float cameraSpeed = 1f;

    public float distance;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // if follow is empty or five seconds have passed, get a random gameobject with the tag 'follow'
        if (follow == null || timePassed > timeUntilSwitch)
        {
            // pick a random gameobject
            int randomIndex = Random.Range(0, GameObject.FindGameObjectsWithTag("follow").Length);
            Debug.Log("random index" + randomIndex);
            follow = GameObject.FindGameObjectsWithTag("follow")[randomIndex];
            timePassed = 0f;
        }
        timePassed += Time.deltaTime;
        followBehind = follow.transform.position;


        // smoothly rotate towards follow object
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(followBehind - transform.position), Time.deltaTime * 1f * timePassed);

        distance = Vector3.Distance(followBehind, transform.position);
        // if distance is less than 5, dont change speed
        if (distance > 5f)
        {
           // scale the distance with the following function y=0.00000001x3−0.00008039x2+0.05060377x−0.16480366
        cameraSpeed = 0.00000001f * distance * distance * distance - 0.00008039f * distance * distance + 0.05060377f * distance - 0.16480366f;
        // max speed 5
        if (cameraSpeed > 3f)
        {
            cameraSpeed = 3f;
        }
        }

        // smoothly move towards follow object
        // adhere to the offset
        // scale the speed of the movement antiproductively to the distance
        transform.position = Vector3.Lerp(transform.position, followBehind, Time.deltaTime * cameraSpeed);

    }
}

