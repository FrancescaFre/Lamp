﻿using UnityEngine;

public class Compass : MonoBehaviour {

    public GameObject arrow, altimeter;

    Transform player;
    Vector3 north;
    float planetRadius;

    private void Start()
    {
        north = GameManager.Instance.levelLoaded.entryPoint;
        planetRadius = north.y;
    }

    // Update is called once per frame
    void Update ()
    {
        MoveArrow();
        MoveAltimeter();
    }

    private void MoveArrow()
    {
        player = GameManager.Instance.currentPC.transform;
        Vector3 playerToNorth = north - player.position;
        float angle = Vector3.SignedAngle(player.forward, Vector3.ProjectOnPlane(playerToNorth, player.up), player.up);

        arrow.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void MoveAltimeter()
    {
        altimeter.transform.localPosition = new Vector3(0, player.position.y * 100 / planetRadius, 0);
    }
}
