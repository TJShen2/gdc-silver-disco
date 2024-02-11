using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnergyDisplay : MonoBehaviour
{
    private GameObject player;
    private PlayerController playerScript;
    private TextMeshProUGUI energyDisplay;

    // Start is called before the first frame update
    void Start()
    {
        energyDisplay = GetComponent<TextMeshProUGUI>();

        player = GameObject.Find("Player");
        playerScript = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        print(playerScript.Energy);
        energyDisplay.SetText("Energy: " + playerScript.Energy + " J");
        energyDisplay.ForceMeshUpdate();
    }
}