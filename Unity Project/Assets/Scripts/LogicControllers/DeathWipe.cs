﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathWipe : MonoBehaviour {

    public Button continueButton;

    void Start()
    {
        continueButton = GetComponent<Button>();

        continueButton.onClick.AddListener(() => SceneManager.LoadScene(4));
    }
}
