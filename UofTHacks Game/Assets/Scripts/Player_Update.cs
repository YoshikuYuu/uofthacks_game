using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey;
using CodeMonkey.Utils;
using TMPro;

public class Player_Update : MonoBehaviour
{
    [SerializeField] private Heads_Update headsUpdateWindow;
    [SerializeField] private UI_InputWindow UIUpdateWindow;
    private TextMeshProUGUI playerText;

    public void getAttacked() {
        playerText = transform.Find("playerHealth").GetComponent<TextMeshProUGUI>();
        string[] headAttacks = {"attack", "attack", "attack", "attack", "attack", "attack", "attack"};
        for (int i = 0; i < headAttacks.Length; i++) {
            if (headAttacks[i] == "attack") {
                int health = int.Parse(playerText.text) - 10;
                playerText.text = health.ToString();
            } else if (headAttacks[i] == "heal") {
                
            }
        }
        UIUpdateWindow.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
