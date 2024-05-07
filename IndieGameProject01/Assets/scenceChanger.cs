using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class scenceChanger : MonoBehaviour
{
    public float countTime = 0f;


    // Update is called once per frame
    void Update()
    {
        CountTime();
    }

    void CountTime()
    {
        countTime += Time.deltaTime;
        if (countTime>68.0f)
        {
            SceneManager.LoadScene(10);

        }
    }

}
