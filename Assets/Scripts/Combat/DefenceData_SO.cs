using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Data", menuName = "Attack/Defence Data")]
public class DefenceData_SO : ScriptableObject
{

    public int defence;

    public void ApplyRightWeaponData(DefenceData_SO shiled)
    {
        defence = shiled.defence;

    }

    public void ApplyRightWeaponData(DefenceData_SO shiled, int type)
    {
        defence = shiled.defence;
    }
}
