﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIcontroller : MonoBehaviour
{
    bool menuflag;
    public GameObject panel;

    void Start()
    {
        menuflag = true;
        panel.SetActive(menuflag);

    }
    public void EXITtoHome()
    {
        SceneManager.LoadScene("HOMEui");
        Time.timeScale = 1;
    }
    public void Arcade()
    {
        SceneManager.LoadScene("LEVEL1");

    }

    public void PRACTICE()
    {
        SceneManager.LoadScene("PRACTICE");

    }
    public void GameOver()
    {
        SceneManager.LoadScene("GAMEOVER");

    }
    public void exit()
    {
        Application.Quit();
    }
    public void Menu()
    {
        menuflag = !menuflag;
        panel.SetActive(menuflag);
        if(menuflag)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    
    void Update()
    {
        
    }
}
