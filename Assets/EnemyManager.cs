using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int numberOfEnemies = 5;

    void Start()
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            Vector3 randomPosition = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), 0);
            GameObject newEnemy = Instantiate(enemyPrefab, randomPosition, Quaternion.identity);

            AIChase aiChase = newEnemy.GetComponent<AIChase>();
            int randomType = Random.Range(0, 3);
            aiChase.enemyType = (AIChase.EnemyType)randomType;
        }
    }
}
