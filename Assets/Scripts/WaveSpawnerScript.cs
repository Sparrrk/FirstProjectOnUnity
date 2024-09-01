using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveSpawnerScript : MonoBehaviour
{
    [System.Serializable]
    public struct Wave
    {
        public EnemyScript[] enemies;
        public int enemiesCount;
        public float timeBtwSpawn;
    }

    [SerializeField] Wave[] waves;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] TextMeshProUGUI textMesh;
    [SerializeField] ParticleSystem spawnEffect;
    public int waveIndex = -1;
    private readonly float timeBtwWaves = 10f;
    private Wave currentWave;
    private Transform player;
    private bool spawnIsFinished = true;
    private bool isFreeTime = false;
    private float timerBtwWaves;

    public void Start()
    {
        timerBtwWaves = timeBtwWaves;
        player = PlayerScript.instance.transform;
    }


    private void Update()
    {
        TextUpdate();
        if (spawnIsFinished && GameObject.FindGameObjectsWithTag("enemy").Length == 0)
        {
            spawnIsFinished = false;
            waveIndex++;
            StartCoroutine(CallTheWave());
        }

    }

    private void TextUpdate()
    {
        if (isFreeTime)
        {
            timerBtwWaves -= Time.deltaTime;
            textMesh.text = "До следующей волны:" + ((int)timerBtwWaves).ToString();
        }
        else
        {
            textMesh.text = "Текущая волна: " + waveIndex.ToString();
        }
    }

    IEnumerator CallTheWave()
    {
        isFreeTime = true;
        yield return new WaitForSeconds(timeBtwWaves);
        isFreeTime = false;
        timerBtwWaves = timeBtwWaves;
        StartCoroutine(SpawnTheWave());
    }


    IEnumerator SpawnTheWave()
    {
        currentWave = waves[waveIndex];

        for (int i = 0; i < currentWave.enemiesCount; i++)
        {
            if (player == null) yield break;

            EnemyScript enemy = currentWave.enemies[UnityEngine.Random.Range(0, currentWave.enemies.Length)];
            Transform spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
            Instantiate(spawnEffect, spawnPoint.position, Quaternion.identity);
            Instantiate(enemy, spawnPoint.position, Quaternion.identity);

            if (i == currentWave.enemiesCount - 1)
            {
                spawnIsFinished = true;
            }
            else
            {
                spawnIsFinished = false;
            }

            yield return new WaitForSeconds(currentWave.timeBtwSpawn);
        }
    }
}
