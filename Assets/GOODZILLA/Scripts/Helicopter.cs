using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helicopter : MonoBehaviour
{
    public Transform target;
    public float radius = 2f;
    public float angularSpeed = 0.15f;

    private Rigidbody body;
    // Start is called before the first frame update
    void Awake()
    {
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Quaternion q = Quaternion.AngleAxis(angularSpeed, Vector3.up);
        Vector3 pos = target.transform.position;
        pos = new Vector3(pos.x, pos.y +  Mathf.Sin(Time.time) * Random.Range(0.001f, 0.01f), pos.z);
        body.MovePosition(q * Vector3.Normalize(transform.position - pos) *  radius + target.position);
        body.MoveRotation(transform.rotation * q);
        transform.LookAt(transform.position + Vector3.Cross(Vector3.ProjectOnPlane(target.position - transform.position, Vector3.up), Vector3.up));
    }


}
