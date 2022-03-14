using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Data", menuName ="Attack/Attack Data")]
public class AttackData_SO : ScriptableObject
{
    public float attackRange;
    public float skillRange;
    public float coolDown;
    public int minDamage;
    public int maxDamage;
    public float criticalMultiplier;
    public float criticalChance;


    public void ApplyWeaponData(AttackData_SO weapon) {
        attackRange = weapon.attackRange;
        skillRange = weapon.skillRange;
        coolDown = weapon.coolDown;

        minDamage = weapon.minDamage;
        maxDamage = weapon.maxDamage;

        criticalMultiplier = weapon.criticalMultiplier;
        criticalChance = weapon.criticalChance;
    
    }


    /// <summary>
    /// When aplly a waepon, call this method. The attack data will be re-calculated.
    /// </summary>
    /// <param name="weapon">The weapon which will be applied.</param>
    /// <param name="type">"0" means equip. "1" means unequip. </param>
    public void ApplyWeaponData(AttackData_SO weapon, int type)
    {
        attackRange = weapon.attackRange;
        skillRange = weapon.skillRange;
        coolDown = weapon.coolDown;

        minDamage = weapon.minDamage;
        maxDamage = weapon.maxDamage;

        criticalMultiplier = weapon.criticalMultiplier;
        criticalChance = weapon.criticalChance;

    }
}
