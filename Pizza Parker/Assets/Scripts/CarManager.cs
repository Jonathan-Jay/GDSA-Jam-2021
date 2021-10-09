using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarManager : MonoBehaviour
{
    public float minCountdown = 5;
    public float maxCountdown = 10;
    private float countdown;
    public GameObject carPrefab;

    [System.Serializable]
    public struct SpawnPoint
    {
        public Transform position;
        public Vector2 direction;
    }

    public List<SpawnPoint> spawnPoints;

    void Start()
    {
        countdown = Random.Range(minCountdown, maxCountdown);
    }

    void Update()
    {
        //randomized counter system
        countdown -= Time.deltaTime;
        if (countdown < 0)
        {
            countdown = Random.Range(minCountdown, maxCountdown);
            //spawn system
            int index = Random.Range(0, spawnPoints.Count);
            Instantiate(carPrefab, spawnPoints[index].position.position, Quaternion.identity)
                .GetComponent<Cars>().dir = spawnPoints[index].direction;
        }
    }
}
