using UnityEngine;

namespace Script.MVC.Module.Physics
{
    public class Phys : MonoBehaviour
    {
        private Transform tra;
        private Vector3 pos = new Vector3();
        private void Awake()
        {
            tra = this.transform;
        }
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            //if () { }
            pos = tra.position;
            pos.y -= Time.deltaTime*2;
            tra.position = pos;
            //FLb.SetPosition(gameObject, pos); 
        }
    }
}
