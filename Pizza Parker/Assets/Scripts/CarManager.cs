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
	private List<Cars> carsAlive = new List<Cars>();
	public Vector2 lowerBound = Vector2.one * -10f;
	public Vector2 upperBound = Vector2.one * 10f;
	public SceneController sceneController;

    [System.Serializable]
    public struct SpawnPoint
    {
        public Transform position;
        public Vector3 direction;
    }

    public List<SpawnPoint> spawnPoints;
	static float winCounter = 0;
	static Vector3 endPos = Vector3.up * 10f;
    static Transform target = null;

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

		if (winCounter > 0f) {
			Camera.main.transform.LookAt(target);

			winCounter -= Time.deltaTime;
			if (winCounter <= 0f) {
				winCounter = 0f;
				target = null;

				sceneController.OnGameEnded();
			}
		}

        //randomized counter system
        countdown -= Time.deltaTime;
        if (countdown < 0)
        {
            countdown = Random.Range(minCountdown, maxCountdown);
            //spawn system
            int index = Random.Range(0, spawnPoints.Count);
            carsAlive.Add(Instantiate(carPrefab,
				spawnPoints[index].position.position,
				spawnPoints[index].position.rotation
			).GetComponent<Cars>());

			carsAlive[carsAlive.Count - 1].dir = spawnPoints[index].direction;
        }

		//bound check
		for (int i = 0; i < carsAlive.Count;) {
			Cars car = carsAlive[i];
			if (car.transform.position.x < lowerBound.x ||
				car.transform.position.x > upperBound.x ||
				car.transform.position.z < lowerBound.y ||
				car.transform.position.z > upperBound.y)
			{
				carsAlive.RemoveAt(i);
				Destroy(car.gameObject);
			}
			else {
				++i;
			}
		}
    }

	public static void WinThing(GameObject newTarget) {
		if (winCounter == 0f) {
			winCounter = 5f;
			Camera.main.transform.position = endPos;
			target = newTarget.transform;
		}
	}
}
