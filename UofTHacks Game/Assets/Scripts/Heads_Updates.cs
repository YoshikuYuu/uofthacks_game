using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey;
using CodeMonkey.Utils;
using TMPro;

public class Heads_Update : MonoBehaviour
{
    [SerializeField] private Player_Update playerUpdateWindow;
    private Image[] fireHeads;
    private Image[] iceHeads;
    private TextMeshProUGUI[] fireTexts;

    public void loadHeads() {
        string[] heads = {"ice", "fire", "ice", "fire", "fire", "ice", "fire"};
        fireHeads = new Image[7];
        iceHeads = new Image[7];
        fireTexts = new TextMeshProUGUI[7];
        for (int i = 0; i < 7; i++) {
            fireHeads[i] = transform.Find("fire" + i).GetComponent<Image>();
            iceHeads[i] = transform.Find("ice" + i).GetComponent<Image>();
            if (string.Equals(heads[i], "fire")) {
                fireHeads[i].enabled = true;
                iceHeads[i].enabled = false;
            } else if (string.Equals(heads[i], "ice")) {
                fireHeads[i].enabled = false;
                iceHeads[i].enabled = true;
            }
            fireTexts[i] = transform.Find("firetxt" + i).GetComponent<TextMeshProUGUI>();
        }
    }

    public void getAttacked() {
        string[] playerAttacks = {"attack", "fire", "ice", "heal", "attack"};
        for (int i = 0; i < playerAttacks.Length; i++) {
            Debug.Log(fireTexts[i].text);
            if (playerAttacks[i] == "attack") {
                int health = int.Parse(fireTexts[i].text) - 10;
                fireTexts[i].text = health.ToString();
            } else if (playerAttacks[i] == "fire") {
                int health = int.Parse(fireTexts[i].text) + 10;
                fireTexts[i].text = health.ToString();
            } else if (playerAttacks[i] == "heal") {

            }
        }
        playerUpdateWindow.getAttacked();
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
