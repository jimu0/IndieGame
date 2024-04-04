using System.Collections;
using System.Collections.Generic;
using Script.MVC.Module.Class;
using Script.MVC.Module.Frame;
using UnityEngine;

public class CollisionTrigger : MonoBehaviour
{
    [SerializeField] public GameObject owner;//?????????????????
    private Gladiatus gladiatus;//??????????

    protected Vector3 Col_home = new Vector3(0, -10000, 0);//????????��??
    //protected Vector3 Tsf_value = new Vector3();//?????????3?��??
    protected Vector2 Col_value = new Vector2();//?????????2?��??????????boxCollider.offset???boxCollider.size?
    BoxCollider2D boxCollider;//????????????

    private void Awake()
    {
        gladiatus = owner.GetComponent<Gladiatus>();
        gameObject.transform.localPosition = Col_home;
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        
    }

    void Update()
    {

    }

    //?????????????????????????????????????tag??layer???��???????????????????????????��
    /// <summary>
    /// ?????????
    /// </summary>
    /// <param name="collision">???????</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != owner.gameObject.tag && collision.gameObject.layer == 9)
        {
            //collision.gameObject.GetComponent<Gladiatus>().Be_Hit(owner, 1);
            Initialization.Instance.Dic_Gladiatus[collision.gameObject].Be_Hit(owner, 1);
            Col_OFF();
        }
        

    }

    //----------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// ????????????
    /// </summary>
    /// <param name="offsetX">???X?</param>
    /// <param name="offsetY">???Y?</param>
    /// <param name="sizeX">???X?</param>
    /// <param name="sizeY">???Y?</param>
    protected void Col_SetValue(float offsetX, float offsetY, float sizeX, float sizeY)
    {
        gameObject.transform.localPosition = Vector3.zero;
        Col_value.x = offsetX; Col_value.y = offsetY;
        boxCollider.offset = Col_value;
        Col_value.x = sizeX; Col_value.y = sizeY;
        boxCollider.size = Col_value;
    }
    /// <summary>
    /// ???????????
    /// </summary>
    public void Col_OFF()
    {
        gameObject.transform.localPosition = Col_home;
        Col_value.x = 0; Col_value.y = 0;
        boxCollider.offset = Col_value;
        Col_value.x = 0.01f; Col_value.y = 0.01f;
        boxCollider.size = Col_value;
    }
    /// <summary>
    /// ????????????
    /// </summary>
    /// <param name="offsetX">???X?</param>
    /// <param name="offsetY">???Y?</param>
    /// <param name="sizeX">???X?</param>
    /// <param name="sizeY">???Y?</param>
    public void Col_ON(float offsetX, float offsetY, float sizeX, float sizeY) 
    {
        Col_SetValue(offsetX, offsetY, sizeX, sizeY);
    }


}
