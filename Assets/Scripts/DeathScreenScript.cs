using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreenScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI currentWaveText;
    [SerializeField] GameObject waveSpawner;

    private void Start()
    {
        currentWaveText.text = "Current Wave:" + waveSpawner.GetComponent<WaveSpawnerScript>().waveIndex.ToString();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
