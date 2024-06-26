using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] LayerMask whatIsPlayer;
    private GameObject player;
    private PlayerController playerScript;
    private LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        
        player = GameObject.Find("Player");
        playerScript = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 1; i < lineRenderer.positionCount; i++)
        {
            Vector2 direction = (lineRenderer.GetPosition(i) - lineRenderer.GetPosition(i - 1)).normalized;
            float distance = (lineRenderer.GetPosition(i) - lineRenderer.GetPosition(i - 1)).magnitude;
            RaycastHit2D ray = Physics2D.Raycast(lineRenderer.GetPosition(i - 1), direction, distance, whatIsPlayer);
            if (ray.collider != null && !playerScript.IsDead) {
                playerScript.Die();
            }
        }
    }
}
