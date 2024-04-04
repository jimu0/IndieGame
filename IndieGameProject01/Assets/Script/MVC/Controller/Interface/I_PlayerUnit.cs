using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_PlayerUnit
{
    /// <summary>
    /// X?????
    /// </summary>
    /// /// <param name="x">x???</param>
    void Move(float x);
    /// <summary>
    /// ??????
    /// </summary>
    /// <param name="y">y???</param>
    void Squat(float y);
    /// <summary>
    /// ????
    /// </summary>
    void Defend();
    /// <summary>
    /// ???????
    /// </summary>
    void Defend_Cancel();
    /// <summary>
    /// ????
    /// </summary>
    void Attack();
    /// <summary>
    /// ???????
    /// </summary>
    void Attack_Cancel();
    /// <summary>
    /// ????????
    /// </summary>
    void Flash();
    void Jump();
}
