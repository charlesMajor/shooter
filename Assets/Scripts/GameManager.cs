using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float timeBetweenAlienSpawn;
    [SerializeField] private int maxAliensInGame;
    [SerializeField] private int maxSpawnedAliens;

    [SerializeField] private PlayerController player;
    [SerializeField] private TMP_Text[] uiTexts = new TMP_Text[4];

    private float timeSinceLastSpawn = 0;
    private int currentAliensInGame = 0;
    private int currentSpawnedAliens = 0;
    private GameObject[] alienSpawners;

    [SerializeField] private GameObject alienPrefab;
    [SerializeField] private int alienPoolSize;
    private ObjectPool alienPool;

    private bool isGameOver = false;
    private bool isGamepadPresent = true;

    [SerializeField] private AudioClip victory;
    private AudioSource audio;

    void Start()
    {
        audio = GetComponent<AudioSource>();
        audio.Play();

        alienPool = new ObjectPool(alienPrefab, alienPoolSize);

        updateSpawnerList();

        StartCoroutine(CheckGamepadPresence());
        UpdateUi();
    }

    void Update()
    {
        currentAliensInGame = GameObject.FindGameObjectsWithTag("Alien").Length;

        timeSinceLastSpawn += (1 * Time.deltaTime);
        if (timeSinceLastSpawn >= timeBetweenAlienSpawn)
        {
            timeSinceLastSpawn = 0;
            spawnAlien();
        }
    }

    public void SetGameOver(bool isGameOver)
    {
        this.isGameOver = isGameOver;
        UpdateUi();

        if (isGameOver)
        {
            StartCoroutine("FadeMusic");
        }
        
    }

    public void UpdateUi()
    {
        /*uiTexts[0].text = player.GetLivesLeft().ToString();
        uiTexts[1].text = player.GetMissileLeft().ToString();
        uiTexts[2].text = player.GetBonusTimeLeft().ToString();
        uiTexts[3].enabled = isGameOver;*/
    }

    public bool IsGamepadPresent()
    {
        return isGamepadPresent;
    }

    public void updateSpawnerList()
    {
        alienSpawners = GameObject.FindGameObjectsWithTag("AlienSpawner");
    }

    private void spawnAlien()
    {
        currentSpawnedAliens++;
        currentAliensInGame++;

        if (currentSpawnedAliens <= maxSpawnedAliens && currentAliensInGame <= maxAliensInGame && alienSpawners.Length != 0)
        {
            int randomSpawner = Random.Range(0, alienSpawners.Length);
            alienPool.GetObjectFromPool(alienSpawners[randomSpawner].transform.position, Quaternion.identity);
        }
    }

    private IEnumerator CheckGamepadPresence()
    {
        while (true)
        {
            int numJoysticks = Input.GetJoystickNames().Length;
            isGamepadPresent = false;

            for (int i = 0; i < numJoysticks; i++)
            {
                if (Input.GetJoystickNames()[i].Length > 0)
                {
                    isGamepadPresent = true;
                    break;
                }
            }

            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator FadeMusic()
    {
        for (float time = 0; time < 3; time += (1 * Time.deltaTime))
        {
            audio.volume = (0.5f - (time / 6));
            print(audio.volume);
            yield return null;
        }

        audio.Stop();
        audio.clip = victory;
        audio.volume = 0.5f;
        audio.loop = false;
        audio.Play();
    }
}