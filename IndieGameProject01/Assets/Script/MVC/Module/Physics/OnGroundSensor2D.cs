using UnityEngine;

namespace Script.MVC.Module.Physics
{
    public class OnGroundSensor2D : MonoBehaviour
    {
        public CapsuleCollider2D capcol;

        private Vector2 CC_offset;//������ƫ�Ʋ���
        private Vector2 CC_size;//������ߴ����
        private CapsuleDirection2D CC_direction;//�����巽�����

        void Awake()
        {
            SetValue();
        }
        void Start()
        {
            //Debug.Log(CC_size.x);
        }

        void FixedUpdate()
        {
            SetValue();
            Collider2D[] outputcols = Physics2D.OverlapCapsuleAll(CC_offset, CC_size, CC_direction, 0,LayerMask.GetMask("Ground"));
            if (outputcols.Length != 0) { SendMessageUpwards("CC_isGround"); }
            else { SendMessageUpwards("CC_isNotGround"); }
        
        }

        public void SetValue() 
        {
            CC_direction = capcol.direction;
            CC_size = capcol.size * 0.5f;
            CC_offset.x = transform.position.x;
            CC_offset.y = transform.position.y - CC_size.y / 2;
        }

    }
}
