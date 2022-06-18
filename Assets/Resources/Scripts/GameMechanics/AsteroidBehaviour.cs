using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEntities;

public class AsteroidBehaviour : MonoBehaviour
{
    public Asteroid asteroid;
    public GameObject asteroidPrefab;
    public Vector3 initialSpeed;
    public int grade;
    public Vector3 dir;

    void Start()
    {
        transform.position = UnityToNumerics.NtoU(asteroid.getPosition());
    }

    void Update()
    {
        asteroid.Update(Time.deltaTime);
        transform.position = UnityToNumerics.NtoU(asteroid.getPosition());
        if (asteroid.destroy)
        {
            List<Asteroid> asteroids = asteroid.segments;
            if (asteroids != null)
            {
                foreach (Asteroid a in asteroids)
                {
                    GameObject asterObject = Instantiate(asteroidPrefab);
                    asterObject.GetComponent<AsteroidBehaviour>().asteroid = a;
                    asterObject.transform.localScale = transform.localScale / 2;
                }
            }
            Destroy(gameObject);
        }
    }
}
