using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransitionScreenController : MonoBehaviour {

    public Button continueButton;
    public Text transitionScreenText;

    private int previousScene;
    private int nextScene;

    private string transitionScreenString = "";

	void Start()
    {
        DetermineScene();

        transitionScreenString = "";

        transitionScreenText = GetComponentInChildren<Text>();

        continueButton = GetComponentInChildren<Button>();

        continueButton.onClick.AddListener(() => SceneManager.LoadScene(nextScene));
	}

    void DetermineScene()
    {
        previousScene = PlayerPrefs.GetInt("PreviousScene");
        nextScene = previousScene + 1;
        Debug.Log("Loading Scene: " + nextScene);
    }

    void Update()
    {
        SetTransitionScreenText();
    }

    void SetTransitionScreenText()
    {
        switch(nextScene)
        {
            case 4:
                transitionScreenString = "Transition to Level 1";
                break;
            case 5:
                transitionScreenString = @"
Terry always loved the view from the top of these mountains. 
You can see the entire countryside from up here. Well, what’s left of it, anyway. 
Between the zombies and the bandits, the towns around here have been completely wiped out. 
I doubt there’s a single person or thing down there that would hesitate to kill me on sight.
 
I should go.The infection is spreading, and who knows what it’s like back in Reah.Hell, it might be worse than it is here. 
Mom and Dad… I can’t stand to lose them too. Maybe if I get there soon enough, I can get them out of this country, somewhere safe. 
That might buy us a little more time together.";
                break;
            case 6:
                transitionScreenString = "Transition to Apartment";
                break;
            case 7:
                transitionScreenString = "Transition to End Credits";
                break;
        }

        transitionScreenText.text = transitionScreenString.Trim();
    }
}
