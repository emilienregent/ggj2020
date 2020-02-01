using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Text TimerTextUI;

    public GameObject[] JoinButtons;
    public GameObject[] LeaveButtons;

    public float StartCountDownTimer = 6;

    private float _timeLeftBeforeStart;
    private bool _countDownStarted;
    private int _totalPlayerReady;

    private bool _canLeave;

    private void Awake()
    {
        try
        {
            for (int i = 0; i < JoinButtons.Length; i++)
            {
                JoinButtons[i].SetActive(true);
            }

            for (int j = 0; j < LeaveButtons.Length; j++)
            {
                LeaveButtons[j].SetActive(false);
            }
        } catch(Exception e)
        {
            Debug.LogException(e, this);
        }

        ResetCountDown();
        _totalPlayerReady = 0;
        _canLeave = true;
    }

    // Update is called once per frame
    void Update()
    {

        // Get inputs
        if(Input.GetButtonDown("Join1") == true)
        {
            PlayerJoin(0);
        }
        if(Input.GetButtonDown("Join2") == true)
        {
            PlayerJoin(1);
        }
        if(Input.GetButtonDown("Join3") == true)
        {
            PlayerJoin(2);
        }
        if(Input.GetButtonDown("Join4") == true)
        {
            PlayerJoin(3);
        }

        if (Input.GetButtonDown("Leave1") == true)
        {
            PlayerLeave(0);
        }
        if (Input.GetButtonDown("Leave2") == true)
        {
            PlayerLeave(1);
        }
        if (Input.GetButtonDown("Leave3") == true)
        {
            PlayerLeave(2);
        }
        if (Input.GetButtonDown("Leave4") == true)
        {
            PlayerLeave(3);
        }

        if(_totalPlayerReady == 0)
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
                _canLeave = false;
            }

            if(_timeLeftBeforeStart < 0)
            {
                // Hide title screen
                Debug.Log("Hide title screen");
                ResetCountDown();
                gameObject.SetActive(false);
            }
        }
    }

    public void ResetCountDown()
    {
        _canLeave = true;
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

    private void PlayerJoin(int id)
    {
        if(JoinButtons[id].gameObject.activeSelf == false)
        {
            return;
        }

        _totalPlayerReady++;
        JoinButtons[id].SetActive(false);
        LeaveButtons[id].SetActive(true);
        StartCountDown();

    }

    private void PlayerLeave(int id)
    {
        if (LeaveButtons[id].gameObject.activeSelf == false || _canLeave == false)
        {
            return;
        }

        _totalPlayerReady--;
        LeaveButtons[id].SetActive(false);
        JoinButtons[id].SetActive(true);
        StartCountDown();

    }


}
