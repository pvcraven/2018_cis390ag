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
    }

    public void SetHealth()
    {
        Debug.Log("Setting Health");
        statusBarInformation.TryGetValue("Health", out statusBarHealth);

        int.TryParse(statusBarHealth, out health);

        // Not sure what this is...
        //currentHealthPanelMax = ((currentHealthPanelMax - healthPanelMin)/(100 * health)) + healthPanelMin;

        float max_health = 100; // This should be a member of something... but it is just "100" everywhere 
        float new_width_of_panel = -( (1-(health / max_health)) * 160);
        healthPanel.offsetMax = new Vector2(new_width_of_panel, -0); // new Vector2(-right, -top);

        Debug.Log(new_width_of_panel);
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

    public void SetWeapon()
    {
        statusBarInformation.TryGetValue("AttackType", out statusBarAttackType);
        if (statusBarAttackType == "melee")
        {
            gunStatus.SetActive(false);
            knifeStatus.SetActive(true);
        } else
        {
            gunStatus.SetActive(true);
            knifeStatus.SetActive(false);
        }
    }
}
