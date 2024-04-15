using UnityEngine;

namespace Script.MVC.Module.Physics
{
    public class OnGroundSensor2D : MonoBehaviour
    {
        public CapsuleCollider2D capcol;

        private Vector2 offset;//������ƫ�Ʋ���
        private Vector2 size;//������ߴ����
        private CapsuleDirection2D direction;//�����巽�����

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
            Collider2D[] outputcols = Physics2D.OverlapCapsuleAll(offset, size, direction, 0,LayerMask.GetMask("Ground"));
            if (outputcols.Length != 0) { SendMessageUpwards("CC_isGround"); }
            else { SendMessageUpwards("CC_isNotGround"); }
        
        }

        public void SetValue() 
        {
            direction = capcol.direction;
            size = capcol.size * 0.5f;
            Vector3 position = transform.position;
            offset.x = position.x;
            offset.y = position.y - size.y / 2;
        }

    }
}
