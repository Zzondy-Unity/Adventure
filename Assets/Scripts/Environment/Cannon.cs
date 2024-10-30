using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField] private Transform From;
    [SerializeField] private Transform To;

    [SerializeField] private FireButton FireButton;
    [SerializeField] private UpDownButton UpButton;
    [SerializeField] private UpDownButton DownButton;

    [SerializeField] private MeshCollider cannonCollider;

    private bool isReadyToFire;
    private float CannonRotRate = 1f;
    public float CannonPower = 50f;
    private Rigidbody playerRB;

    Vector3 startPos, endPos;
    LineRenderer lineRenderer;


    private void Start()
    {
        FireButton.FireButtonClicked += Fire;
        UpButton.UpDownButtonClick += CannonRot;
        DownButton.UpDownButtonClick += CannonRot;
        playerRB = CharacterManager.Instance.player.rb;
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
    }



    private void Fire()
    {
        if (isReadyToFire)
        {
            //5초동안 치지직 소리, 5초후 펑소리
            StartCoroutine(FireOnFiveSeconds());
        }
    }

    private IEnumerator FireOnFiveSeconds()
    {
        yield return new WaitForSeconds(3);
        Vector3 FireForce = (To.position - playerRB.position).normalized * CannonPower;

        if (playerRB != null)
        {
            playerRB.AddForce(FireForce + playerRB.transform.forward * CannonPower * 2, ForceMode.Impulse);
        }

        Debug.DrawLine(playerRB.position, playerRB.position + FireForce * 5, Color.black, 5f);
    }


    private void CannonRot(int dir)
    {
        //누르면 Y축 회전이 0 ~ 90도로
        transform.Rotate(0, CannonRotRate * dir, 0);
    }

    private void OnTriggerStay(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            isReadyToFire = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {


        if (other.CompareTag("Player"))
        {
            StopCoroutine(FireOnFiveSeconds());
            isReadyToFire = false;
        }
    }

}
