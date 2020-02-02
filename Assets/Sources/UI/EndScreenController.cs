using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScreenController : MonoBehaviour
{

    public Text score;

    private void OnEnable()
    {
        GameManager.instance.audioManager.PlayEndScreenMusic(0.5f);
    }

}
