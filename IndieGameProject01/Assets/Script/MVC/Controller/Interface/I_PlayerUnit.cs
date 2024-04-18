using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_PlayerUnit
{
    /// <summary>
    /// X轴
    /// </summary>
    /// /// <param name="x">x轴</param>
    void Move(float x);
    /// <summary>
    /// y轴
    /// </summary>
    /// <param name="y">y轴</param>
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
    /// 闪避
    /// </summary>
    void Flash();
    /// <summary>
    /// 跳跃
    /// </summary>
    void JumpD();
    void Jump();
    void JumpU();
    /// <summary>
    /// 技能1
    /// </summary>
    /// <param name="kd"></param>
    void Skill1(bool kd);
    /// <summary>
    /// 技能2
    /// </summary>
    void Skill2();
    /// <summary>
    /// 技能3
    /// </summary>
    void Skill3();
    /// <summary>
    /// 技能4
    /// </summary>
    void Skill4();
}
