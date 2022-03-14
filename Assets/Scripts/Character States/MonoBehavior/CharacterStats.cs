using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public event Action<int, int> UpdateHealthBarOnAttack;

    public CharacterData_SO templateData;
    public CharacterData_SO characterData;
    public AttackData_SO attackData;
    public DefenceData_SO defenceData;
    private AttackData_SO baseAttackData;
    private RuntimeAnimatorController baseAniamtor;

    [HideInInspector]
    public bool isCritical;


    public string fileName;
    public string characterClass;
    public string characterName;

    [Header("Weapon")]
    public Transform weaponSlot;
    public Transform rightWeaponSlot;

    private bool getExp;

    private void Awake()
    {
        baseAniamtor = GetComponent<Animator>().runtimeAnimatorController;
        if (templateData != null)
        {
            characterData = Instantiate(templateData);
        }
        baseAttackData = Instantiate(attackData);

        getExp = false;
    }

    #region Read from Data_SO
    public int MaxHealth
    {
        get { if (characterData != null) return characterData.maxHealth; else return 0; }
        set { characterData.maxHealth = value; }
    }

    public int CurrentHealth
    {
        get { if (characterData != null) return characterData.currentHealth; else return 0; }
        set { characterData.currentHealth = value; }
    }

    public int BaseDefence
    {
        get { if (characterData != null) return characterData.baseDefence; else return 0; }
        set { characterData.baseDefence = value; }
    }

    public int CurrentDefence
    {
        get { if (characterData != null) return characterData.currentDefence; else return 0; }
        set { characterData.currentDefence = value; }
    }

    public float AttackRange
    {
        get { if (attackData != null) return attackData.attackRange; else return 0; }
        set { attackData.attackRange = value; }
    }

    public float SkillRange
    {
        get { if (attackData != null) return attackData.skillRange; else return 0; }
        set { attackData.skillRange = value; }
    }

    public float CoolDown
    {
        get { if (attackData != null) return attackData.coolDown; else return 0; }
        set { attackData.coolDown = value; }
    }

    public float CriticalMultiplier
    {
        get { if (attackData != null) return attackData.criticalMultiplier; else return 0; }
        set { attackData.criticalMultiplier = value; }
    }


    public float CriticalChance
    {
        get { if (attackData != null) return attackData.criticalChance; else return 0; }
        set { attackData.criticalChance = value; }
    }

    public int MinDamage
    {
        get { if (attackData != null) return attackData.minDamage; else return 0; }
        set { attackData.minDamage = value; }
    }

    public int MaxDamage
    {
        get { if (attackData != null) return attackData.maxDamage; else return 0; }
        set { attackData.maxDamage = value; }
    }

    #endregion

    #region Character Combat
    public void TakeDamage(CharacterStats attacker, CharacterStats defener)
    {
        int damage = Mathf.Max(attacker.CurrentDamage() - defener.CurrentDefence, 0);
        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);

        if (attacker.isCritical && CurrentHealth != 0)
        {
            defener.GetComponent<Animator>().SetTrigger("GetHit");
        }

        // Update UI health bar
        UpdateHealthBarOnAttack?.Invoke(CurrentHealth, MaxHealth);
        // level UPDATE
        if (CurrentHealth <= 0)
        {
            if (attacker.CompareTag("Player") && !getExp)
            {
                attacker.characterData.UpdateExp(characterData.killPoint);
                getExp = true;
            }
        }
    }


    public void TakeDamage(int damage, CharacterStats defender)
    {
        int currentDamage = Mathf.Max(damage - defender.CurrentDefence, 0);
        CurrentHealth = Mathf.Max(CurrentHealth - currentDamage, 0);
        // Update UI
        UpdateHealthBarOnAttack?.Invoke(CurrentHealth, MaxHealth);
        if (CurrentHealth <= 0 && defender.CompareTag("Enemy") && !getExp)
        {
            getExp = true;
            GameManager.Instance.playerStates.characterData.UpdateExp(characterData.killPoint);
        }
    }

    private int CurrentDamage()
    {
        //get the random damage between maxDamage and minDamage
        float coreDamage = UnityEngine.Random.Range(attackData.minDamage, attackData.maxDamage);

        //calculate the damage if the attack has critical hit
        if (isCritical)
        {
            coreDamage *= attackData.criticalMultiplier;
            //Debug.Log("Critical Damage" + coreDamage);
        }

        return (int)coreDamage;
    }

    #endregion

    #region Equip Weapon

    public void ChangeWeapon(ItemData_SO weapon) {

        UnEquipWeapon();
        EquipWeapon(weapon);
        //InventoryManager.Instance.UpdateStatsText(CurrentHealth, attackData.minDamage, attackData.maxDamage);
    }


    internal void ChangeRightWeapon(ItemData_SO rightWeapon)
    {
        UnEquipRightWeapon();
        EquipRightWeapon(rightWeapon);
    }

    public void EquipWeapon(ItemData_SO weapon) {
        if (weapon.weaponPrefab != null) {
            var w = Instantiate(weapon.weaponPrefab, weaponSlot);
            if(w.gameObject.layer != LayerMask.NameToLayer("Player"))
                w.gameObject.layer = LayerMask.NameToLayer("Player");
        }

        //TODO: update attribute 
        //TODO:Switch animation
        //attackData.ApplyWeaponData(weapon.weaponData, 0);
        attackData.ApplyWeaponData(weapon.weaponData);
        GetComponent<Animator>().runtimeAnimatorController = weapon.weaponAnimator;
        //InventoryManager.Instance.UpdateStatsText(CurrentHealth, attackData.minDamage, attackData.maxDamage);
    }

    public void EquipRightWeapon(ItemData_SO weapon)
    {
        if (weapon.ShieldPrefab != null)
        {
            var w = Instantiate(weapon.ShieldPrefab, rightWeaponSlot);
            if (w.gameObject.layer != LayerMask.NameToLayer("Player"))
                w.gameObject.layer = LayerMask.NameToLayer("Player");
        }

        //TODO: update attribute 
        //TODO:Switch animation
        //attackData.ApplyWeaponData(weapon.weaponData, 0);
        //attackData.ApplyWeaponData(weapon.weaponData);
        CurrentDefence += weapon.shieldData.defence;


        GetComponent<Animator>().runtimeAnimatorController = weapon.shieldAnimator;
        //InventoryManager.Instance.UpdateStatsText(CurrentHealth, attackData.minDamage, attackData.maxDamage);
    }


    public void UnEquipWeapon() {

        if (weaponSlot.transform.childCount != 0) {
            for (int i = 0; i < weaponSlot.transform.childCount; i++)
            {
                Destroy(weaponSlot.transform.GetChild(i).gameObject);
            }
        
        }

        attackData.ApplyWeaponData(baseAttackData);
        //TODO:Switch animation
        if (rightWeaponSlot.childCount == 0)
        {
            GetComponent<Animator>().runtimeAnimatorController = baseAniamtor;
            attackData.ApplyWeaponData(baseAttackData);
        }
    }

    public void UnEquipRightWeapon()
    {

        if (rightWeaponSlot.transform.childCount != 0)
        {
            for (int i = 0; i < rightWeaponSlot.transform.childCount; i++)
            {
                Destroy(rightWeaponSlot.transform.GetChild(i).gameObject);
            }

        }

        //attackData.ApplyWeaponData(baseAttackData);
        CurrentDefence = BaseDefence;
        //TODO:Switch animation
        if (weaponSlot.childCount == 0)
        {
            GetComponent<Animator>().runtimeAnimatorController = baseAniamtor;
            attackData.ApplyWeaponData(baseAttackData);
        }
    }
    #endregion

    #region Aplly Data Change

    public void ApplyHealth(int amount) {

        //TODO: if currentHealth == MaxHealth, can not heal

        if (CurrentHealth + amount <= MaxHealth)
            CurrentHealth += amount;
        else
            CurrentHealth = MaxHealth; 
    }


    internal void GetFullStates()
    {
        CurrentHealth = MaxHealth;
        Debug.Log("I get heal and my current Health is: " + CurrentHealth);
    }

    #endregion
}
