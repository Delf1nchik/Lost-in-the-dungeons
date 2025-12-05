using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRoom : MonoBehaviour
{
    [Header("Walls")]
    public GameObject[] walls;
    public GameObject wallEffect;
    public GameObject door;

    [Header("Enemies")]
    public GameObject[] enemyTypes;
    public Transform[] enemySpawners; 

    [Header("Powerups")]
    public GameObject shield;
    public GameObject healthPotion;

    [HideInInspector] public List<GameObject> enemies;

    private bool spawned;
    private bool wallsDestroyed; 

    private void Start()
    {
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Player") && !spawned)
        {
            spawned = true;

            foreach (Transform spawner in enemySpawners)
            {
                int rand = Random.Range(0, 11);
                if (rand < 9) 
                {
                    GameObject enemyType = enemyTypes[Random.Range(0, enemyTypes.Length)];
                    GameObject enemy = Instantiate(enemyType, spawner.position, Quaternion.identity);
                    enemy.transform.parent = transform;
                    enemies.Add(enemy);
                }
            }

            StartCoroutine(CheckEnemies());
        }
    }

    IEnumerator CheckEnemies()
    {
        yield return new WaitForSeconds(1f); 
        yield return new WaitUntil(() => enemies.Count == 0);
        DestroyWalls();
    }

    public void DestroyWalls()
    {
        foreach (GameObject wall in walls)
        {
            if (wall != null && wall.transform.childCount != 0)
            {
                Instantiate(wallEffect, wall.transform.position, Quaternion.identity);
                Destroy(wall);
            }
        }
        wallsDestroyed = true;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (wallsDestroyed && other.CompareTag("Wall"))
        {
            Destroy(other.gameObject);
        }
    }

}
