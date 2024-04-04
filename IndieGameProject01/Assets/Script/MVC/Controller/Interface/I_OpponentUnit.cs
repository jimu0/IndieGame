namespace Script.MVC.Other.Interface
{
    public interface I_OpponentUnit
    {
        /// <summary>
        /// X轴移动
        /// </summary>
        /// /// <param name="x">x轴值</param>
        void Move(float x);
        /// <summary>
        /// 架势高低
        /// </summary>
        /// <param name="y">y轴值</param>
        void Squat(float y);
        /// <summary>
        /// 防御
        /// </summary>
        void Defend();
        /// <summary>
        /// 取消防御
        /// </summary>
        void Defend_Cancel();
        /// <summary>
        /// 攻击
        /// </summary>
        void Attack();
        /// <summary>
        /// 取消攻击
        /// </summary>
        void Attack_Cancel();
        /// <summary>
        /// 快速位移
        /// </summary>
        void Flash();

    }
}
