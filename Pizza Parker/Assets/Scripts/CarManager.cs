using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarManager : MonoBehaviour
{
    public float minCountdown = 5;
    public float maxCountdown = 10;
    private float countdown;
    public GameObject carPrefab;

	public Drunk drunkScript;
	public bool drunkMode = false;
	private List<Cars> carsAlive;
	public Vector2 lowerBound = Vector2.one * -10f;
	public Vector2 upperBound = Vector2.one * 10f;

    [System.Serializable]
    public struct SpawnPoint
    {
        public Transform position;
        public Vector3 direction;
    }

    public List<SpawnPoint> spawnPoints;

    void Start()
	{
		drunkScript.enabled = drunkMode;
		
        countdown = Random.Range(minCountdown, maxCountdown);
    }

    void Update()
	{
		if (Input.GetButtonDown("Cancel"))
		{
			if (drunkMode)
			{
				drunkMode = false;
			}
			else
			{
				drunkMode = true;
			}
			drunkScript.enabled = drunkMode;
		}

        //randomized counter system
        countdown -= Time.deltaTime;
        if (countdown < 0)
        {
            countdown = Random.Range(minCountdown, maxCountdown);
            //spawn system
            int index = Random.Range(0, spawnPoints.Count);
            carsAlive.Add(Instantiate(carPrefab, spawnPoints[index].position.position, Quaternion.identity).GetComponent<Cars>());
			carsAlive[carsAlive.Count - 1].dir = spawnPoints[index].direction;
        }

		//bound check
		foreach (Cars car in carsAlive) {
			if (car.transform.position.x < lowerBound.x ||
				car.transform.position.x > upperBound.x ||
				car.transform.position.z < lowerBound.y ||
				car.transform.position.z > upperBound.y)
			{
				carsAlive.Remove(car);
				Destroy(car);
			}
		}
    }
}
