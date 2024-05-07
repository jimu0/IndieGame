using UnityEngine;

namespace _2DGamekit.Scripts.AI
{
    public class NanguaTou : MonoBehaviour
    {
        public int id;
        public NanguaBoss namguaBoss;
        private void Start()
        {
        
        }

        private void Update()
        {
        }

        public void Die()
        {
            //Debug.Log("Die!");
            switch (id)
            {
                case 1:
                    namguaBoss.head1Alive = false;
                    namguaBoss.Jineng(namguaBoss.hit1, true);
                    break;
                case 2:
                    namguaBoss.head2Alive = false;
                    break;
                case 3:
                    namguaBoss.head3Alive = false;
                    break;
                case 4:
                    namguaBoss.head4Alive = false;
                    break;
                case 5:
                    namguaBoss.head5Alive = false;
                    break;
            }
            namguaBoss.CutOffAHead();
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        }

        public void Hit()
        {
            //Debug.Log("Hit!");
        }

        public void Attack()
        {
            //Debug.Log("Attack!");
        }
    
    }
}
