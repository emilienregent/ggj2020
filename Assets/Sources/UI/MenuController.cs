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
    public GameObject buttonJoin;

    public float StartCountDownTimer = 6;

    private float _timeLeftBeforeStart;
    private bool _countDownStarted;
    private int _totalPlayerReady;

    private bool _canLeave;
    private bool _canUSeInput = false;

    private void Awake()
    {

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
                _canLeave = false;
            }

            if(_timeLeftBeforeStart < 0)
            {
                // Hide title screen
                ResetCountDown();
                gameObject.SetActive(false);
                GameManager.instance.currentGameState = GameManager.enumGameState.Game;
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

    public void PlayerJoin()
    {
        if (_canUSeInput == false)
        {
            return;
        }

        _totalPlayerReady++;
        StartCountDown();

        if(_totalPlayerReady == 4)
        {
            buttonJoin.SetActive(false);
        }

    }


}
