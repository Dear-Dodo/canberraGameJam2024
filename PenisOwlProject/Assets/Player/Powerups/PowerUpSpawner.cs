using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    public List<GameObject> powerUps = new List<GameObject>();


    public float Interval;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnPowerUp());
    }

    private IEnumerator SpawnPowerUp()
    {
        Destroy(Instantiate(powerUps[Random.Range(0, powerUps.Count)], transform.position + Vector3.right * Random.Range(-10f, 10f), Quaternion.identity),5);

        yield return new WaitForSeconds(Random.Range(Interval,Interval * 0.5f));
        StartCoroutine(SpawnPowerUp());
    }
}
