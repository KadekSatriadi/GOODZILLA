using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    public Vector2 playArea;
    public float radiusMin = 10f;
    public float radiusMax = 20f;
    public float spawnMinTime;
    public float spawnMaxTime;
    public float forceMagMin = 0.75f;
    public float forceMagMax = 3f;
    public float meteorSizeMin = 0.1f;
    public float meteorSizeMax = 0.3f;

    public GameObject meteorPrefab;
    private bool isPlaying = false;


    public void StartWave()
    {
        isPlaying = true;
        StartCoroutine(Spawn());
    }

    public void StopWave()
    {
        isPlaying = false;
        StopCoroutine(Spawn());
    }

    public IEnumerator Spawn()
    {
        while (isPlaying)
        {
            yield return new WaitForSeconds(Random.Range(spawnMinTime, spawnMaxTime));

            GameObject g = Instantiate(meteorPrefab);
            //position
            float angle = Random.Range(0f, 360f);
            float r = Random.Range(radiusMin, radiusMax);
            float x = radiusMin * Mathf.Sin(Mathf.Deg2Rad * angle);
            float z = radiusMin * Mathf.Cos(Mathf.Deg2Rad * angle);

            Vector3 position = new Vector3(x, transform.position.y, z);
            g.transform.position = position;

            //direction
            x = Random.Range(playArea.x * -0.5f, playArea.x * 0.5f);
            z = Random.Range(playArea.y * -0.5f, playArea.y * 0.5f);
            Vector3 target = new Vector3(x, 0, z);
            Vector3 force = Vector3.Normalize(target - position);

            //add force
            float randomForce = Random.Range(forceMagMin, forceMagMax);
            g.GetComponent<Meteor>().Go(force * randomForce, force * randomForce);

            //rescale meteor
            g.transform.localScale = Vector3.one * Random.Range(meteorSizeMin, meteorSizeMax);
        }
    }

}
