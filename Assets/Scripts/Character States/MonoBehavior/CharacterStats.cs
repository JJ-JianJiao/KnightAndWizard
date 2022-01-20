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

    [HideInInspector]
    public bool isCritical;

    public string fileName;
    public string characterClass;
    public string characterName;

    private void Awake()
    {
        if (templateData != null) {
            characterData = Instantiate(templateData);
        }
    }

    #region Read from Data_SO
    public int MaxHealth {
        get { if (characterData != null) return characterData.maxHealth; else return 0; }
        set { characterData.maxHealth = value;}
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
    public void TakeDamage(CharacterStats attacker, CharacterStats defener) {
        int damage = Mathf.Max(attacker.CurrentDamage() - defener.CurrentDefence,0);
        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);

        if (attacker.isCritical && CurrentHealth !=0) {
            defener.GetComponent<Animator>().SetTrigger("GetHit");
        }

        // Update UI health bar
        UpdateHealthBarOnAttack?.Invoke(CurrentHealth, MaxHealth);
        // level UPDATE
        if (CurrentHealth <= 0) {
            if(attacker.CompareTag("Player"))
                attacker.characterData.UpdateExp(characterData.killPoint);
        }
    }

    public void TakeDamage(int damage, CharacterStats defender) {
        int currentDamage = Mathf.Max(damage - defender.CurrentDefence, 0);
        CurrentHealth = Mathf.Max(CurrentHealth - currentDamage, 0);
        // Update UI
        UpdateHealthBarOnAttack?.Invoke(CurrentHealth, MaxHealth);
        if (CurrentHealth <= 0 && defender.CompareTag("Enemy"))
            GameManager.Instance.playerStates.characterData.UpdateExp(characterData.killPoint);
    }

    private int CurrentDamage()
    {
        //get the random damage between maxDamage and minDamage
        float coreDamage = UnityEngine.Random.Range(attackData.minDamage, attackData.maxDamage);

        //calculate the damage if the attack has critical hit
        if (isCritical) {
            coreDamage *= attackData.criticalMultiplier;
            //Debug.Log("Critical Damage" + coreDamage);
        }

        return (int)coreDamage;
    }

    #endregion
}
