using System.Collections;
using System.Collections.Generic;
using Script.MVC.Other.Interface;
using UnityEngine;

public class PlayerController_2D : MonoBehaviour
{
    private I_PlayerUnit I_playerUnit;
    private float Horizontal = 0;//xÖáÊäÈë
    private float Vertical = 0;//yÖáÊäÈë
    //public Pawn BP_Player;
    private void Awake()
    {
        I_playerUnit = GetComponent<I_PlayerUnit>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Horizontal = Input.GetAxis("Horizontal");
        Vertical = Input.GetAxis("Vertical");
        if (Horizontal != 0)
        {
            I_playerUnit.Move(Horizontal);
        }
        if (Vertical != 0) 
        {
            I_playerUnit.Squat(Vertical);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            I_playerUnit.Attack();
        }
        if (Input.GetKeyUp(KeyCode.H))
        {
            I_playerUnit.Attack_Cancel();
        }

        if (Input.GetKeyDown(KeyCode.U)) 
        {
            I_playerUnit.Defend();
        }
        if (Input.GetKeyUp(KeyCode.U))
        {
            I_playerUnit.Defend_Cancel();
        }

        if (Input.GetKeyDown(KeyCode.J)) 
        {
            I_playerUnit.Jump();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            I_playerUnit.Flash();
        }

    }
}
