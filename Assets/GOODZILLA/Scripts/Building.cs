using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Building : MonoBehaviour
{
    public float HitPoints = 100;

    private Rigidbody rigidbody;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    //public void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.tag == "Hittable" && HitPoints >= 0)
    //    {
    //        var hittable = other.gameObject.GetComponent<Hittable>();
    //        DealDamage(hittable.Velocity);
    //        var closestPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
    //        hittable.CreateSpark(closestPoint);
    //    }
    //}

    public void DealDamage(Vector3 force)
    {
        Debug.Log(force.magnitude);

        HitPoints -= (force.magnitude);

        if (HitPoints <= 0)
        {
            DestroyBuilding(force);
        }

        //BuildingDamageManager.Instance.AddDamageDone(force.magnitude);
    }

    public void DestroyBuilding(Vector3 force)
    {
        GetComponent<Collider>().isTrigger = false;
        rigidbody.isKinematic = false;
        rigidbody.AddForce(force);

        Invoke("Explode", 2f);
    }

    public void Explode()
    {
        int numRubble = Random.Range(15, 20);

        for (int i = 0; i < 10; i++)
        {
            var rubble = GameObject.CreatePrimitive(PrimitiveType.Cube);

            var pos = transform.position;
            pos.x = pos.x + Random.Range(-0.1f, 0.1f);
            pos.y = pos.y + Random.Range(-0.1f, 0.1f);
            pos.z = pos.z + Random.Range(-0.1f, 0.1f);
            rubble.transform.position = pos;
            rubble.transform.localScale = transform.localScale / (numRubble / 3);

            var rb = rubble.AddComponent<Rigidbody>();
            Vector3 explosionPoint = transform.position;
            explosionPoint.y = 0;
            rb.AddExplosionForce(200, transform.position, 10, 2);
        }

        Destroy(gameObject);
    }
}
