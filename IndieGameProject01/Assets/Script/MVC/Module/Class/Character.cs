using Script.MVC.Module.Collision;
using Script.MVC.Module.Frame;
using UnityEngine;

namespace Script.MVC.Module.Class
{
    public class Character : Pawn
    {

        public GameMode GameMode;
        public bool isGround = false;
        public bool isPlatform = false;
        public BoxCollider2D boxCollider2D;//获取地面的碰撞
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
