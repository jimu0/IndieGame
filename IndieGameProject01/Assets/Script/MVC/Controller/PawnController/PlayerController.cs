using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    //public Pawn BP_Player;
    private void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            float horizontal = Input.GetAxis("Horizontal"); //A D ����
            float vertical = Input.GetAxis("Vertical"); //W S �� ��
            float m_speed = 10f;
            transform.Translate(Vector3.forward * vertical * m_speed * Time.deltaTime);//W S �� ��
            transform.Translate(Vector3.right * horizontal * m_speed * Time.deltaTime);//A D ����}

        }
        
    }
}
