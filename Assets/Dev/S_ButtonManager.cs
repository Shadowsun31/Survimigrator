﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class S_ButtonManager : MonoBehaviour
{

    public string nextScene;
    public GameObject m_MainMenu, m_SelectPlayer, m_Credits;

    public GameObject creditMigrant;

    public Button m_StartGameButton;

    public string textJoin = "Press A to leave your home";

    public Text[] m_Texts;

    public GameObject prefabPlayer;

    public Transform[] spawnPlayer;

    private GameObject[] currentPlayer;


    #region MainMenu

    public void Menu_Start()
    {
        m_SelectPlayer.SetActive( true );
        m_MainMenu.SetActive( false );
    }

    public void Menu_Credits()
    {
        m_Credits.SetActive( true );
        creditMigrant.SetActive( true );
        m_MainMenu.SetActive( false );
    }

    public void Menu_Quit()
    {
        Application.Quit();
    }

    #endregion

  
    void Start()
    {
        for(int i = 0;i< GameData.PlayerMax;i++ )
        {
            GameData.singleton.playerInput[ i ] = -1;
            m_Texts[i].text = textJoin;
        }

        currentPlayer = new GameObject[4];


}

void Update()
    {
        if( m_SelectPlayer.activeInHierarchy )
        {
            if( Input.GetButtonDown( "Cancel" ) )
            {
                m_SelectPlayer.SetActive( false );
                m_MainMenu.SetActive( true );
                m_StartGameButton.Select();

            }

            for (int i = 0; i < 4; i++)
            {
                if (GameData.singleton.playerInput[i] != i  &&Input.GetButtonDown("Joy" + (i + 1) + "_ButA"))
                {
                    GameData.singleton.playerInput[i] = i;

                    m_Texts[i].text = "Player" + (i + 1);

                    currentPlayer[i] = (GameObject) Instantiate(prefabPlayer, spawnPlayer[i].position, Quaternion.identity);
                    currentPlayer[i].GetComponent<MigrantPlayer>().SetNumPlayer(i);
                }
                else if(GameData.singleton.playerInput[i] == i && Input.GetButtonDown("Joy" + (i + 1) + "_ButB"))
                {
                    Destroy(currentPlayer[i]);
                    m_Texts[i].text = textJoin;
                    GameData.singleton.playerInput[i] = -1;

                }
            }
            
            if(Input.GetKey(KeyCode.Joystick1Button7)|| Input.GetKey( KeyCode.Joystick2Button7 ) || Input.GetKey( KeyCode.Joystick3Button7 ) || Input.GetKey( KeyCode.Joystick4Button7 ) )
            {
                SceneLoader.singleton.changeScene( nextScene );
            }

            
        }

        if( m_Credits.activeInHierarchy )
        {
            if( Input.GetButtonDown( "Cancel" ) )
            {
                m_Credits.SetActive( false );
                creditMigrant.SetActive( false );
                m_MainMenu.SetActive( true );
                m_StartGameButton.Select();
                           
            }
        }
      
        

    }


}
