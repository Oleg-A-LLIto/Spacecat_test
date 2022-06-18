using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEntities;
using Physics;

public class GameManagerBehaviour : MonoBehaviour
{
    GameManager gameManager;
    [SerializeField] GameObject asteroidPrefab;
    [SerializeField] GameObject ufoPrefab;
    [SerializeField] GameEndMenuBehaviour gameEndMenu;
    Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerBehaviour>().player;
        float realAsteroidSize = asteroidPrefab.GetComponent<SpriteRenderer>().size.x / 2 * asteroidPrefab.transform.localScale.x;
        float realUFOSize = ufoPrefab.GetComponent<SpriteRenderer>().size.x / 4 * ufoPrefab.transform.localScale.x;
        gameManager = new GameManager(player, realAsteroidSize, realUFOSize);
    }

    // Update is called once per frame
    void Update()
    {
        List<Entity> tospawn = gameManager.Update(Time.deltaTime);
        if(tospawn != null)
        {
            foreach (Entity e in tospawn)
            {
                if (e.objectType() == (int)objectTypes.asteroid)
                {
                    GameObject newAsteroid = Instantiate(asteroidPrefab);
                    newAsteroid.GetComponent<AsteroidBehaviour>().asteroid = (Asteroid)e;
                    break;
                }
                if (e.objectType() == (int)objectTypes.enemy)
                {
                    GameObject newUFO = Instantiate(ufoPrefab);
                    newUFO.GetComponent<UFOBehaviour>().ufo = (UFO)e;
                    break;
                }
            }
        }
        if (gameManager.gameEnded)
        {
            gameEndMenu.showGameEndScreen(gameManager.score);
        }
    }
}
