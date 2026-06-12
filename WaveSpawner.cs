using System.Diagnostics;
using System.Threading.Tasks.Dataflow;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaveSpawner : MonoBehavior
{
    [System.Serializable]
    public class Wave
    {
        public GameObject[] enemyPrefabs;
        public int enemyCount = 5;
        public float spawnInerval = 2f;
    }

    public Wave[] waves;
    public float timeBetweenWaves = 5f;
    public int currentWaveIndex = 0;
    public TMP_Text waveText;
    
    void Start()
    {
        PlayerPrefs.DeleteKey("LastWave");
        currentWaveIndex = PlayerPrefs.GetInt("LastWave", 0);
        Debug.Log("Last wave index retrieved: " + currentWaveIndex);
        StartCoroutine(ReleaseWaves());
    }


    IEnumerator ReleaseWaves()
    {
        while(currentWaveIndex < waves.Length)
        {
            //Debug.Log("Wave" + (currentWaveIndex + 1) + "Incoming!");
            UpdateWaveText();
            yield return new WaitForSeconds(timeBetweenWaves);
            //Debug.Log("Spawning enemies in this wave...");
            yield return StartCoroutine(SpawnWave(waves[currentWaveIndex]));

            //Debug.Log("Waiting for all Enemies to be destroyed...");
            //yield return new WaitUntil(AreAllEnemiesDestroyed);

            yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Enemy").Length == 0);
            currentWaveIndex++;

            PlayerPrefs.SetInt("LastWave", currentWaveIndex);
            PlayerPrefs.Save();
            Debug.Log("Last Wave is "+ currentWaveIndex);
        }
    }
    void Update()
    {
        
    }

    IEnumerator SpawnWave(Wave wave)
    {
        for(int i=0; i < wave.enemyCount; i++)
        {
            int enemyIndex = Random.Range(0, wave.enemyPrefabs.Length);
            GameObject enemyPrefab = wave.enemyPrefabs[enemyIndex];
            SpawnEnemy(enemyPrefab);
            yield return new WaitForSeconds(wave.spawnInerval);
        }
    } 

    void SpawnEnemy(GameObject enemyPrefab)
    {
        Instantiate(enemyPrefab, transform.position, transform.rotation);
    }

    bool AreAllEnemiesDestroyed()
    {
        return GameObject.FindGameObjectsWithTag("Enemy").Length == 0;
    }
    void UpdateWaveText()
    {
        if (waveText)
        {
            waveText.text = (currentWaveIndex + 1).ToString() + " / " + waves.Length.ToString();
        }
    }
}