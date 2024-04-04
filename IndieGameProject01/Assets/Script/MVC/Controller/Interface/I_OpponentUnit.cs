namespace Script.MVC.Other.Interface
{
    public interface I_OpponentUnit
    {
        /// <summary>
        /// X���ƶ�
        /// </summary>
        /// /// <param name="x">x��ֵ</param>
        void Move(float x);
        /// <summary>
        /// ���Ƹߵ�
        /// </summary>
        /// <param name="y">y��ֵ</param>
        void Squat(float y);
        /// <summary>
        /// ����
        /// </summary>
        void Defend();
        /// <summary>
        /// ȡ������
        /// </summary>
        void Defend_Cancel();
        /// <summary>
        /// ����
        /// </summary>
        void Attack();
        /// <summary>
        /// ȡ������
        /// </summary>
        void Attack_Cancel();
        /// <summary>
        /// ����λ��
        /// </summary>
        void Flash();

    }
}
