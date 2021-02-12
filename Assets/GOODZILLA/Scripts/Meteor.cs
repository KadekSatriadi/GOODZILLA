using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    public float damage;
    public ParticleSystem meteorTrailParticles;
    public GameObject explosionParticleSystemPrefab;
    public AudioSource meteorBurning;
    public AudioSource meteorExplosion;

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
        var force = collision.impulse / Time.fixedDeltaTime * transform.localScale.x * 100;
        var damage = Mathf.Max(force.magnitude, 20);

        switch (collision.gameObject.tag)
        {
            case "Building":
                var building = collision.gameObject.GetComponent<Building>();
                Vector3 collisionDirection = (collision.contacts[0].point - transform.position).normalized;
                building.DealDamage(collisionDirection, damage);
                StartCoroutine(Explode());
                break;

            case "Floor":
                StartCoroutine(Explode());
                gameManager.AddDamage(damage);
                break;

            case "PlayerCollider":
            default:
                StartCoroutine(Explode(0));
                break;
        }

    }

    private IEnumerator Explode(float delay = 0)
    {
        yield return new WaitForSeconds(delay);

        // Play particle effects
        var particles = Instantiate(explosionParticleSystemPrefab).GetComponent<ParticleSystem>();
        particles.transform.position = transform.position;
        particles.Play();
        Destroy(particles.gameObject, 2f);

        // Play explosion sound effect
        meteorBurning.Stop();
        meteorExplosion.Play();

        // Destroy this meteor on a delay so that the sound effect can play
        gameObject.GetComponent<Renderer>().enabled = false;
        gameObject.GetComponent<Collider>().enabled = false;
        body.isKinematic = true;
        meteorTrailParticles.Stop();
        Destroy(gameObject, 4f);
    }
}
