using System.Collections;
using Script.MVC.Module.Collision;
using Script.MVC.Module.Frame.ObjectPool;
using Script.MVC.Other.Timer2;
using UnityEngine;
namespace Script.MVC.Module.Class
{
    public class Gladiatus : Character
    {
        /// <summary>
        /// ��Ϊ״̬
        /// </summary>
        public enum Behavior
        {
            /// <summary>
            /// վ��
            /// </summary>
            Stand,
            /// <summary>
            /// ����
            /// </summary>
            Defend,
            /// <summary>
            /// ����
            /// </summary>
            Attack,
            /// <summary>
            /// �ܻ�
            /// </summary>
            BeAttackde,
            /// <summary>
            /// �м�
            /// </summary>
            Parry,
            /// <summary>
            /// ��ֱ
            /// </summary>
            Rigidity,
            /// <summary>
            /// ����
            /// </summary>
            Die
        }
        /// <summary>
        /// ��λ״̬
        /// </summary>
        public enum Buff { }
        /// <summary>
        /// Ŀ��
        /// </summary>
        public GameObject _targetObject = null;
        /// <summary>
        /// ��ǰ��Ϊ״̬
        /// </summary>
        public Behavior behavior = Behavior.Stand;

        //����
        /// <summary>
        /// ����ֵ
        /// </summary>
        public float _HP;
        /// <summary>
        /// ����ֵ
        /// </summary>
        public float _SP;
        /// <summary>
        /// ����
        /// </summary>
        public float strength;
        /// <summary>
        /// ����
        /// </summary>
        public float armor;
        /// <summary>
        /// //����
        /// </summary>
        public float agility;
        /// <summary>
        /// ����
        /// </summary>
        public float attackSpeed = 0.1f;
        /// <summary>
        /// ����
        /// </summary>
        public float moveSpeed;
        /// <summary>
        /// ������Ӧ�ٶ�
        /// </summary>
        public float reactionSpeed = 0.1f;
        /// <summary>
        /// ��������
        /// </summary>
        public struct Attr { }



        //�Ƕ�ʿ��ǰ��Ϊ״̬

        public GameObject _Obj_ams;//��ʱ����ɫ���ƿ��ӻ�
        public Transform _Tsf_ams;//��ʱģ���ֱ۵�Transform
        public Vector3 _ams_pos = new Vector3(0.4f, 0, 0);//��ʱ��ģ���ֱ۶�����boxĬ��posֵ
        public SpriteRenderer _ams_SpR;//��ʱ����ɫ�������ӻ��������
        Color DefendForward_color = new Color(255, 255, 255, 1);//��ʱ����ɫ�������ӻ�_��ɫ
        public int squatValue = 0;//����ID
        public bool longPress_Defend = false;//�����������
        private float squatYO;//squatUpdate�еĵ����ж�
        public Timer timer_SquatUp;//����̧��ǰҡʱ��
        public Timer timer_SquatDoun;//���Ʒ���ǰҡʱ��
        public Timer timer_Squat;//���Ʒ��غ�ҡʱ��
        public Timer timer_AttackForward;//����ǰҡʱ��
        public Timer timer_AttackBack;//������ҡʱ��
        public Timer timer_DefendForward;//����ǰҡʱ��

        public GameObject _Obj_ColTrigger;//��ײ������Obj
        public CollisionTrigger _CollisionTrigger;//������Ϊ����ײ������
        public bool Attack_Trigger = false;//�Ƿ����������
        public float Move_SpeedAttenuation = 1;//�ƶ��ٶ�˥������
        private int orient = 1;//��ǰ����
        private int orient_Preset = 1;//��ǰ����Ԥ��ֵ
        private Vector3 OrientValue = new Vector3(1,1,1);//����ֵ
        public bool OrientSwitch = false;//����ת�򿪹�
        public bool TargetLocked = false;//Ŀ������״̬

        public float force = 5; //��Ծ�߶�
        //private bool isGrounded = false;//�Ƿ���½
        
        
        private void Awake()
        {
            //_Tsf_ams = _Obj_ams.transform;
            //_ams_SpR = _Obj_ams.GetComponent<SpriteRenderer>();
            //reactionSpeed = 0.1f;
            //attackSpeed = 0.2f;
            ////if (behavior == Behavior.stand) { }
        }

        void Start()
        {
            //ConstructionTimer();
        
        }




        

        void Update()
        {
            //Land();
            //_Tsf_ams.localPosition = _ams_pos;
            //if (Col_AttackTrigger) { }
        }




        /// <summary>
        /// ��ʼ����ʱ������
        /// </summary>
        public void ConstructionTimer()
        {
            timer_SquatUp = Timer.Start(reactionSpeed, (float timeUpdata) => { _ams_pos.y = 0.3f * (timeUpdata / reactionSpeed); }, () => { squatValue = 1; }, 0.01f);// Debug.Log("���ƣ�̧��"); 

            timer_SquatDoun = Timer.Start(reactionSpeed, (float timeUpdata) => { _ams_pos.y = -0.3f * (timeUpdata / reactionSpeed); }, () => { squatValue = -1; }, 0.01f);// Debug.Log("���ƣ�ѹ��"); 

            timer_Squat = Timer.Start(reactionSpeed, (float timeUpdata) => { _ams_pos.y = 0.3f * squatValue * ((reactionSpeed - timeUpdata) / reactionSpeed); }, () => { squatValue = 0; }, 0.01f);// Debug.Log("���ƣ�ƽʽ"); 

            timer_DefendForward = Timer.Start(attackSpeed, (float timeUpdata) =>
                {
                    DefendForward_color.b = 255 - 255 * (timeUpdata / attackSpeed);
                    _ams_SpR.color = DefendForward_color;
                    //timer_SquatUp.Pause(); timer_Squat.Pause(); timer_SquatDoun.Pause();
                },
                () =>
                {
                    if (behavior == Behavior.Defend)
                    {
                        DefendForward_color.b = 0;
                        _ams_SpR.color = DefendForward_color;
                    }
                    else
                    {
                        DefendForward_color.b = 255;
                        _ams_SpR.color = DefendForward_color;
                    }
                    //timer_AttackBack.ReStart(attackSpeed);
                },
                0.01f);// Debug.Log("����������"); 

            timer_AttackBack = Timer.Start(attackSpeed, (float timeUpdata) =>
                {
                    _ams_pos.x = 0.4f + ((attackSpeed - timeUpdata) / attackSpeed);
                    timer_SquatUp.Resume(); timer_Squat.Resume(); timer_SquatDoun.Resume();
                },
                () =>
                {
                    OrientSwitch = true;
                    Move_SpeedAttenuation = 1;
                    if (longPress_Defend) {  SetOrient(orient_Preset, false); ReadyDefend(); } else { behavior = Behavior.Stand; }
                },
                0.01f);//Debug.Log("״̬���أ�����"); 

            timer_AttackForward = Timer.Start(attackSpeed, (float timeUpdata) =>
                {
                    Move_SpeedAttenuation = 1;
                    _ams_pos.x = 4f * (timeUpdata / attackSpeed);
                    timer_SquatUp.Pause(); timer_Squat.Pause(); timer_SquatDoun.Pause();
                },
                () =>
                {
                    TriggerAttack(attackSpeed);
                    timer_AttackBack.ReStart(attackSpeed/10);
                },
                0.01f);// Debug.Log("����������"); 
            
            

        }
        /// <summary>
        /// ת������
        /// </summary>
        /// <param name="i">X��ֵ����</param>
        /// <param name="TargetLocked">Ŀ������״̬</param>
        public void SetOrient(float i,bool TargetLocked)
        {

            if (TargetLocked) 
            {
                orient_Preset = orient; 
            } 
            else 
            {
                //Ԥ��ֵ(ֻҪ��������û����Ӧ�ͼ�¼����Ԥ��,���ڹ�����ҡ����ʱ��longPress_Defend����Ϊtrueʱ�����ô�Ԥ�賯��Ϊ�³���)
                if (!longPress_Defend)
                {
                    if (i > 0) { orient_Preset = 1; }
                    else if (i < 0) { orient_Preset = -1; }
                    else { orient_Preset = 1; }
                }


                if (OrientSwitch)
                {
                    //if (orient != orient_Preset) { orient = orient_Preset; }
                    if (i > 0)
                    { orient = 1; OrientValue.x = orient; gameObject.transform.localScale = OrientValue; }
                    else if (i < 0)
                    { orient = -1; OrientValue.x = orient; gameObject.transform.localScale = OrientValue; }
                    else { orient = 1; OrientValue.x = orient; gameObject.transform.localScale = OrientValue; }
                }
            }

        }

        /// <summary>
        /// ׼�������ж�
        /// </summary>
        /// <param name="y">y������</param>
        public void judgeTheSquat(float y)
        {
            if (y >= 0.34)
            { //�����ж�
                if (squatYO >= 0.34) { }
                else
                {
                    timer_Squat.Cancel();
                    timer_SquatDoun.Cancel();
                    timer_SquatUp.ReStart(reactionSpeed);
                    squatYO = y;
                }
            }
            else if (y <= -0.34)
            { //�����ж�
                if (squatYO <= -0.34) { }
                else
                {
                    timer_SquatUp.Cancel();
                    timer_Squat.Cancel();
                    timer_SquatDoun.ReStart(reactionSpeed);
                    squatYO = y;
                }
            }
            else
            { //�м��ж�
                if (squatYO > -0.34 && squatYO < 0.34) { }
                else
                {
                    timer_SquatUp.Cancel();
                    timer_SquatDoun.Cancel();
                    timer_Squat.ReStart(reactionSpeed);
                    squatYO = y;
                }
            }
            //Debug.Log(y.ToString());
        }

        /// <summary>
        /// ׼����Ծ
        /// </summary>
        public void ReadyJump(Rigidbody2D rig, float jumpForce) 
        {
            //if (float.Parse(string.Format("{0:F1}", transform.position.y)) > 0.5f) return;
            if (behavior == Behavior.Stand) 
            {
                //Debug.Log("jump!");
                rig.velocity += Vector2.up * (force + jumpForce);
            }
        }


        /// <summary>
        /// ׼������
        /// </summary>
        public void ReadyAttack()
        {
            DefendForward_color.b = 255;
            _ams_SpR.color = DefendForward_color;
            if (behavior == Behavior.Stand || behavior == Behavior.Defend)
            {
                OrientSwitch = false;
                behavior = Behavior.Attack;
                timer_AttackForward.ReStart(attackSpeed);
                //Move_SpeedAttenuation = 0.01f;
            }
        }
        /// <summary>
        /// ׼������
        /// </summary>
        public void ReadyDefend()
        {
        
            if (behavior == Behavior.Stand)
            {
                OrientSwitch = false;
                behavior = Behavior.Defend;
                timer_DefendForward.ReStart(attackSpeed);
            }
            if (behavior == Behavior.Attack && longPress_Defend)
            {
                OrientSwitch = false;
                behavior = Behavior.Defend;
                timer_DefendForward.ReStart(attackSpeed);
            }
        }
        /// <summary>
        /// ȡ������
        /// </summary>
        public void CancelDefend()
        {

            if (behavior == Behavior.Defend)
            {
                timer_DefendForward.Cancel();
                OrientSwitch = true;
                behavior = Behavior.Stand;
                DefendForward_color.b = 255;
                _ams_SpR.color = DefendForward_color;
            }
            //Debug.Log("������ȡ������");
        }
        /// <summary>
        /// ������������
        /// </summary>
        /// <param name="time">����ʱ��/s</param>
        private void TriggerAttack(float time) 
        {
            Attack_Trigger = true;
            _CollisionTrigger.Col_ON(2.1f*orient,0,4.2f,0.8f);
            StartCoroutine(CancelAttack(time));
        }
        /// <summary>
        /// ͨ��Э�̣��ӳ�ֹͣ��������
        /// </summary>
        /// <param name="time">����ʱ��/s</param>
        /// <returns>�ӳ�ʱ��</returns>
        private IEnumerator CancelAttack(float time) 
        {
            yield return new WaitForSecondsRealtime(time);
            //yield return null;
            _CollisionTrigger.Col_OFF();
            Attack_Trigger = false;
        }
        /// <summary>
        /// ������ײ�����Ӧ
        /// </summary>
        /// <param name="collision">��ײ����</param>
        public void Attack_Hit(Collider2D collision)
        {
        
            if (Attack_Trigger)//Attack_Trigger
            {
                //_targetObject = collision.gameObject;//�����ж������õ�Ŀ�굥λ
                //Debug.Log("��");
                Attack_Trigger = false;

            }
        }
        /// <summary>
        /// �ܻ���ײ�����Ӧ
        /// </summary>
        /// <param name="Source">�˺���Դ��λ</param>
        /// <param name="Damage">�˺�ֵ</param>
        public void Be_Hit(GameObject Source,float Damage) 
        {
            if (_HP-Damage <= 0)
            {
                _HP = 0;
                Die();
                return;
            }
            _HP -= Damage;
            Debug.Log("�ܵ����ԡ�"+ Source.name+"�����˺���ʣ������ֵ��"+_HP.ToString()+"��");
        }
        /// <summary>
        /// ��������
        /// </summary>
        public void Die()
        {
            Debug.Log(this.name+"����");
        }

        

        void OnTriggerEnter2D(Collider2D other)
        {
            // �������������ײ���ǵ������ײ��
            if (other.CompareTag("Ground"))
            {
                isGround = true;
                Debug.Log("OnTriggerExit2D:Grounded");
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            // ����뿪�˵������ײ��
            if (other.CompareTag("Ground"))
            {
                isGround = false;
                Debug.Log("OnTriggerExit2D:Floating");
            }
        }

        public LayerMask groundLayer; // ��1����7λ����ʾֻ����7ͼ��
        public void Land()
        {
            Vector2 pos = Vector2.down;
            pos.x = transform.position.x;
            pos.y = transform.position.y;
            RaycastHit2D[] hits = Physics2D.RaycastAll(pos, Vector2.down, 0.6f);
            isGround = false;
            foreach (var hit in hits)
            {
                if (hit.collider != null && hit.collider.gameObject.layer == 6)
                {
                    isGround = true; // �����ײ��Layer 6
                    break; // ������ҵ�Layer 6�����壬�����һ�����
                }
            }
            Color rayColor = isGround ? Color.red : Color.green;
            Debug.DrawRay(pos, Vector2.down * 0.6f, rayColor);
        }
        
        public void Fire(Timer fireTime,bool sticking,Rigidbody2D fireBulletRig)
        {
            //bulletObj = bulletPool.Get();
            //bulletBox = fireBulletBox;
            //bulletRig = fireBulletRig;
            
            fireTime.ReStart();
        }

    }
}
