using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    public float damage;
    public GameObject particleSystemPrefab;

    private float selfDestroyTime = 10f;
    private Rigidbody body;
    private GameManager gameManager;
    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        body = GetComponent<Rigidbody>();
    }
    
    public void Go(Vector3 force, Vector3 rotation)
    {
        body.AddForce(force);
        body.AddTorque(rotation);
    }

    public void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.tag == "PlayerCollider")
        //{
            StartCoroutine(Explode(0f));
        //}
    }

    public void OnCollisionEnter(Collision collision)
    {
        var damage = collision.impulse * transform.localScale.x * 10000f;

        switch (collision.gameObject.tag)
        {
            case "Building":
                var building = collision.gameObject.GetComponent<Building>();
                building.DealDamage(damage);
                StartCoroutine(Explode());
                gameManager.AddDamage(damage.magnitude);
                
                break;

            case "Floor":
                StartCoroutine(Explode());
                gameManager.AddDamage(damage.magnitude);
                break;

            case "PlayerCollider":
                break;

            default:
                StartCoroutine(Explode(2f));
                body.useGravity = true;
                break;
        }

    }

    private IEnumerator Explode(float delay = 0)
    {
        yield return new WaitForSeconds(delay);

        // Play particle effects
        var particles = Instantiate(particleSystemPrefab).GetComponent<ParticleSystem>();
        particles.transform.position = transform.position;
        particles.Play();
        Destroy(particles.gameObject, 2f);

        // Destroy this meteor
        Destroy(gameObject);
    }
}
