﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBarLogic : MonoBehaviour
{

    private Player player;
    public GameObject playerGameObject;
    public GameObject statusBarGameObject;
    private IDictionary<string, string> statusBarInformation = new Dictionary<string, string>();

    private string statusBarHealth = "100";
    private string statusBarStamina = "500";
    private string statusBarAttackType = "melee";
    private string statusBarStrength = "0";

    private int health = 100;

    public Transform healthPanel;
    public Transform stausPanel;

    private float healthPanelMin;
    private float initialHealthPanelMax;

    private float currentHealthPanelMax;




    void Start()
    {
        healthPanelMin = healthPanel.GetComponent<RectTransform>().anchorMin.x;
        initialHealthPanelMax = healthPanel.GetComponent<RectTransform>().anchorMax.x;
        currentHealthPanelMax = initialHealthPanelMax;

        Debug.Log("HERE" + healthPanel.GetComponent<RectTransform>().anchorMax.x);
    }

    void Update()
    {
        statusBarInformation = GetComponent<PlayerController>().tory.GetStatusBarInformation;
        Debug.Log(statusBarInformation["Health"]);
    }

    public void SetHealth()
    {
        Debug.Log("Setting Health");
        statusBarInformation.TryGetValue("Health", out statusBarHealth);

        int.TryParse(statusBarHealth, out health);

        currentHealthPanelMax = ((currentHealthPanelMax - healthPanelMin)/(100 * health)) + healthPanelMin;

        Debug.Log("I\n" + initialHealthPanelMax);
        Debug.Log("C\n" + currentHealthPanelMax);
    }

    public void SetStamina()
    {
        statusBarInformation.TryGetValue("Stamina", out statusBarStamina);
    }

    void SetAttackType()
    {
        statusBarInformation.TryGetValue("AttackType", out statusBarAttackType);
    }

    void SetStrength()
    {
        statusBarInformation.TryGetValue("Strength", out statusBarStrength);
    }
}
