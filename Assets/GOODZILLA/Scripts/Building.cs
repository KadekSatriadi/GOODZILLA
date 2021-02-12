using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Building : MonoBehaviour
{
    public float HitPoints = 100;
    public AudioSource buildingDamaged;
    public AudioSource buildingFalling;
    public AudioSource buildingExplosion;
    public GameObject sparkParticlePrefab;

    private Rigidbody rigidbody;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        rigidbody = GetComponent<Rigidbody>();

        Vector3 pos = transform.position;
        pos.y = transform.localScale.y / 2;
        transform.position = pos;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerHands")
        {
            // Get closest point to hands
            Vector3 closestPoint = GetComponent<BoxCollider>().ClosestPoint(other.transform.position);
            // Get direction vector
            Vector3 direction = (transform.position - closestPoint).normalized;

            DealDamage(direction, 10);
            gameManager.AddDamage(10);

            var sparks = Instantiate(sparkParticlePrefab) as GameObject;
            sparks.transform.position = closestPoint;
            sparks.GetComponent<ParticleSystem>().Play();
            Destroy(sparks, 2f);
        }
    }

    public void DealDamage(Vector3 collisionDirection, float damage)
    {
        HitPoints -= (damage);
        gameManager.AddDamage(damage);

        if (HitPoints <= 0)
        {
            DestroyBuilding(collisionDirection, damage);
        }
        else
        {
            buildingDamaged.Play();
            StartCoroutine(ShakeBuilding(0.05f, damage, 0.75f));
        }
    }

    public IEnumerator ShakeBuilding(float amount, float speed, float duration)
    {
        float timer = 0;

        Vector3 originalPos = transform.position;

        while (timer < duration)
        {
            Vector3 pos = transform.position;
            if (Random.Range(0, 1) % 2 == 0)
            {
                pos.x = originalPos.x + Mathf.Sin(timer * speed) * amount;
            }
            else
            {
                pos.z = originalPos.z + Mathf.Sin(timer * speed) * amount;
            }
            transform.position = pos;

            yield return null;

            timer += Time.deltaTime;
        }

        transform.position = originalPos;
    }

    public void DestroyBuilding(Vector3 force, float damage)
    {
        GetComponent<Collider>().isTrigger = false;
        rigidbody.constraints = RigidbodyConstraints.None;
        rigidbody.AddForce(force * damage * 1.5f);
        buildingFalling.Play();
        Invoke("Explode", 2f);
    }

    public void Explode()
    {
        Vector3 rubbleSize = Vector3.one * Random.Range(0.15f, 0.2f);

        int numWidth = Mathf.FloorToInt(transform.localScale.x / rubbleSize.x);
        int numHeight = Mathf.FloorToInt(transform.localScale.y / rubbleSize.y);
        int numDepth = Mathf.FloorToInt(transform.localScale.z / rubbleSize.z);

        for (int i = 0; i < numWidth; i++)
        {
            for (int j = 0; j < numHeight; j++)
            {
                for (int k = 0; k < numDepth; k++)
                {
                    var rubble = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    rubble.transform.localScale = rubbleSize;

                    Vector3 pos;
                    pos.x = (i * rubbleSize.x) - (transform.localScale.x / 2);
                    pos.y = (j * rubbleSize.y) - (transform.localScale.y / 2);
                    pos.z = (k * rubbleSize.z) - (transform.localScale.z / 2);
                    rubble.transform.SetParent(transform);
                    rubble.transform.localPosition = pos;
                    rubble.transform.rotation = transform.rotation;
                    rubble.transform.parent = null;
                    rubble.transform.localScale = rubbleSize;

                    var rb = rubble.AddComponent<Rigidbody>();
                    Vector3 explosionPoint = transform.position;
                    explosionPoint.y = 0;
                    rb.AddExplosionForce(50, transform.position, 10, 1);

                    Destroy(rubble, 10f);
                }
            }
        }

        buildingExplosion.Play();
        gameObject.GetComponent<Renderer>().enabled = false;
        gameObject.GetComponent<Collider>().enabled = false;
        rigidbody.isKinematic = true;
        Destroy(gameObject, 4f);
    }
}
