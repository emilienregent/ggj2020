using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class StartMenuController : MonoBehaviour
{
    public Text TimerTextUI;
    public GameObject buttonJoin;

    public float StartCountDownTimer = 6;

    private float _timeLeftBeforeStart;
    private bool _countDownStarted;
    private int _totalPlayerReady;

    private void Awake()
    {

        ResetCountDown();
        _totalPlayerReady = 0;

    }

    private void Start()
    {
        GameManager.instance.audioManager.PlayStartScreenMusic(1f);
    }

    private void OnDisable()
    {
        GameManager.instance.audioManager.Stop(2f);
    }

    private void EnableInput()
    {
        GetComponent<Animator>().SetBool("Idle", true);
    }

    // Update is called once per frame
    void Update()
    {

        if (_totalPlayerReady == 0)
        {
            ResetCountDown();
        }

        if(_countDownStarted == true)
        {
            _timeLeftBeforeStart -= Time.deltaTime;

            int seconds = (int)_timeLeftBeforeStart % 60;

            if(seconds > 0)
            {
                TimerTextUI.text = seconds.ToString();
            } else
            {
                TimerTextUI.text = "Go !!";
            }

            if(_timeLeftBeforeStart < 0)
            {
                // Hide title screen
                ResetCountDown();
                gameObject.SetActive(false);
                GameManager.instance.StartGame();
            }
        }
    }

    public void ResetCountDown()
    {
        _countDownStarted = false;
        _timeLeftBeforeStart = StartCountDownTimer;
        TimerTextUI.gameObject.SetActive(false);
        TimerTextUI.text = StartCountDownTimer.ToString();
    }

    public void StartCountDown()
    {
        _countDownStarted = true;
        _timeLeftBeforeStart = StartCountDownTimer;
        TimerTextUI.gameObject.SetActive(true);
        TimerTextUI.text = StartCountDownTimer.ToString();
    }

    public void PlayerJoin()
    {

        _totalPlayerReady++;
        StartCountDown();

        if(_totalPlayerReady == 4)
        {
            buttonJoin.SetActive(false);
        }

    }


}
