using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
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
    private bool _canUSeInput = false;

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

        // Listen InputManager event
        //PlayerInputManager.PlayerJoinedEvent _playerJoinedEvent = new PlayerInputManager.PlayerJoinedEvent();
        //_playerJoinedEvent.AddListener(PlayerJoin);
    }

    //private void PlayerJoin(PlayerInput arg0)
    //{
    //    throw new NotImplementedException();
    //}

    //public void OnSubmit(InputAction.CallbackContext context)
    //{
    //    Debug.Log(context);
    //}

    private void EnableInput()
    {
        _canUSeInput = true;
        GetComponent<Animator>().SetBool("Idle", true);
    }

    // Update is called once per frame
    void Update()
    {

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

    public void PlayerJoin(PlayerController player)
    {
        if (_canUSeInput == false || JoinButtons[player.currentPlayerIndex].gameObject.activeSelf == false)
        {
            return;
        }

        _totalPlayerReady++;
        JoinButtons[player.currentPlayerIndex].SetActive(false);
        LeaveButtons[player.currentPlayerIndex].SetActive(true);
        StartCountDown();

    }

    public void PlayerLeave(PlayerController player)
    {
        if (_canUSeInput == false || LeaveButtons[player.currentPlayerIndex].gameObject.activeSelf == false || _canLeave == false)
        {
            return;
        }

        _totalPlayerReady--;
        LeaveButtons[player.currentPlayerIndex].SetActive(false);
        JoinButtons[player.currentPlayerIndex].SetActive(true);
        StartCountDown();

    }


}
