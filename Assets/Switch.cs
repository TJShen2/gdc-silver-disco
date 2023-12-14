using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    private GameObject laser;
    [SerializeField] private KeyCode activateSwitchKey;
    [SerializeField] private LayerMask player;
    // Start is called before the first frame update
    void Start()
    {
        laser = GameObject.Find("Laser");
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics2D.BoxCast(transform.position, new Vector2(1,1), 0, new Vector2(0,0), 0, player)) {
            if (Input.GetKeyDown(activateSwitchKey) && laser.activeSelf) {
                laser.SetActive(false);
            } else if (Input.GetKeyDown(activateSwitchKey)) {
                laser.SetActive(true);
            }
        }
    }
}