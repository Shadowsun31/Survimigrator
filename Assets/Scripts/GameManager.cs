﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public static GameManager singleton;

    public CanvasGroup gameOverTitle;
    public string sceneGameOver;

    public int nbrMigrantMin;
    public int nbrMigrantMax;
    private int nbrMigrant;

    private float startTime;

    public Text timeLeftDisplay;

    public GameObject PrefabsMigrant;
    public GameObject PrefabsMigrantPlayer;

    public Transform spawnA;
    public Transform spawnB;
    public List<Transform> spawnPlayer;

    private bool pictureTook;

    public void Awake()
    {
        singleton = this;
        gameOverTitle.alpha = 0;
        pictureTook = false;
    }

    public void Start()
    {
        GameData.singleton.ResetGameData();
        GenerateMigrant();
        StartCoroutine(timerToGameOver());
        startTime = Time.time;
        pictureTook = false;
        //GameData.singleton.camTexture = new WebCamTexture();
        //GameData.singleton.camTexture.Play();
    }

    public void playerDie(int numPlayer)
    {
        GameData.singleton.playerAlive[numPlayer] = false;
        GameData.singleton.playerTimeSurvive[numPlayer] = Time.time - startTime;

        bool StillPlayerAlive = false;
        for(int i = 0;i < GameData.PlayerMax; i++)
        {
            if(GameData.singleton.playerInput[i] != -1 && GameData.singleton.playerAlive[i])
            {
                StillPlayerAlive = true;
            }
        }
        if (!StillPlayerAlive)
        {
            StartCoroutine(gameOver());
        }
    }

    public void Update()
    {
        float timeLeft = GameData.singleton.TimeGame - (Time.time - startTime);
        timeLeftDisplay.text = "Time Left : "+timeLeft.ToString("00");

        if (!pictureTook)
        {
            if(Time.time - startTime > 15)
            {
                pictureTook = true;
                //GameData.singleton.camTexture.Play();
                //GameData.singleton.camTexture.Pause();
                StartCoroutine(takePicture());
            }
        }
    }

    public IEnumerator takePicture()
    {
        GameData.singleton.camTexture.Play();
        yield return new WaitForSeconds(2f);
        GameData.singleton.camTexture.Pause();

    }

    public IEnumerator timerToGameOver()
    {
        yield return new WaitForSeconds(GameData.singleton.TimeGame);
        StartCoroutine(gameOver());
    }

    public IEnumerator gameOver()
    {
        float startTime = Time.time;
        float normTime = 0;
        while(normTime < 1)
        {
            normTime = (Time.time - startTime) / 1;
            gameOverTitle.alpha = normTime;
            yield return null;
        }
        yield return new WaitForSeconds(2);

        SceneLoader.singleton.changeScene(sceneGameOver);

    }


    public void GenerateMigrant()
    {
        nbrMigrant = Random.Range(nbrMigrantMin, nbrMigrantMax);
        GameData.singleton.nbrMigrantTotal = nbrMigrant;

        for(int i = 0;i < nbrMigrant;i++)
        {
            Vector3 spawnPosition = Vector3.Lerp(spawnA.position, spawnB.position, ((float)i) / nbrMigrant);
            GameObject tempMigrant = (GameObject) Instantiate(PrefabsMigrant, spawnPosition, Quaternion.identity);
        }

        for(int i = 0; i < GameData.PlayerMax; i++)
        {
            if(GameData.singleton.playerInput[i] != -1)
            {
                int rnd = Random.Range(0, spawnPlayer.Count);
                GameObject tempPlayer = (GameObject)Instantiate(PrefabsMigrantPlayer, spawnPlayer[rnd].position, Quaternion.identity);

                spawnPlayer.RemoveAt(rnd);

                tempPlayer.GetComponent<MigrantPlayer>().SetNumPlayer(i);

            }
        }
           /* public GameObject PrefabsMigrant;
    public GameObject PrefabsMigrantPlayer;*/

    }
}
