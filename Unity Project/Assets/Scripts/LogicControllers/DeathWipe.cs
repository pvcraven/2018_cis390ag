using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathWipe : MonoBehaviour
{

    public Button continueButton;

    void Start()
    {
        continueButton = GetComponent<Button>();

        PlayerPrefs.SetInt("PreviousScene", 3);

        continueButton.onClick.AddListener(() => SceneManager.LoadScene(3));
    }
}
