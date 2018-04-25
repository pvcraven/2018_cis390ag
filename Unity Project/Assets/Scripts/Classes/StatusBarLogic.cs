using System.Collections;
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
    private float stamina = 500;

    public RectTransform healthPanel;
    public RectTransform stausPanel;
    public GameObject gunStatus;
    public GameObject knifeStatus;

    private float healthPanelMin;
    private float initialHealthPanelMax;

    private float currentHealthPanelMax;




    void Start()
    {
        healthPanelMin = healthPanel.GetComponent<RectTransform>().anchorMin.x;
        initialHealthPanelMax = healthPanel.GetComponent<RectTransform>().anchorMax.x;
        currentHealthPanelMax = initialHealthPanelMax;

        gunStatus.SetActive(false);
        knifeStatus.SetActive(false);

        Debug.Log("HERE" + healthPanel.GetComponent<RectTransform>().anchorMax.x);
    }

    void Update()
    {
        statusBarInformation = GetComponent<PlayerController>().tory.GetStatusBarInformation;
        SetHealth();
    }

    public void SetHealth()
    {
        // Debug.Log("Setting Health");
        statusBarInformation.TryGetValue("Health", out statusBarHealth);

        int.TryParse(statusBarHealth, out health);

        // Not sure what this is...

        //currentHealthPanelMax = ((currentHealthPanelMax - healthPanelMin)/(100 * health)) + healthPanelMin;

        //This ^ mathematically works out how much of the health bar to remove based on the length of the bar. (For Res-Scaling)
        //Unless you can get the current method to actually run out of the health bar in the correct amount of hits to kill you, we need to adapt the equation below to match something similar.
        //Currently, it does not.

        float max_health = 100; // This should be a member of something... but it is just "100" everywhere 
        float new_width_of_panel = -( (1-(health / max_health)) * 220);

        healthPanel.offsetMax = new Vector2(new_width_of_panel, -0); // new Vector2(-right, -top);
    }

    public void SetStamina()
    {
        statusBarInformation.TryGetValue("Stamina", out statusBarStamina);

        float.TryParse(statusBarStamina, out stamina);

        float max_stamina = 500; // This should be a member of something... but it is just "500" everywhere 
        float new_width_of_panel = -((1 - (stamina / max_stamina)) * 220);
        stausPanel.offsetMax = new Vector2(new_width_of_panel, -0); // new Vector2(-right, -top);
    }

    void SetAttackType()
    {
        statusBarInformation.TryGetValue("AttackType", out statusBarAttackType);
    }

    void SetStrength()
    {
        statusBarInformation.TryGetValue("Strength", out statusBarStrength);
    }

    public void SetWeapon()
    {
        statusBarInformation.TryGetValue("AttackType", out statusBarAttackType);

        if (statusBarAttackType == "melee")
        {
            gunStatus.SetActive(false);
            knifeStatus.SetActive(true);
        }
        else if (statusBarAttackType == "ranged")
        {
            gunStatus.SetActive(true);
            knifeStatus.SetActive(false);
        }
    }
}
