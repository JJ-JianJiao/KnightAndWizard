using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { Useable, Weapon, Armor, Quest }
[CreateAssetMenu(fileName ="New Item", menuName = "Inventory/Item Data")]
public class ItemData_SO : ScriptableObject
{
    public ItemType itemType;
    public string itemName;
    public Sprite itemIcon;
    public int itemAmount;
    public bool isTwoHandWeapn;
    [TextArea]
    public string description = "";

    public bool stackable;

    [Header("Weapon")]
    public GameObject weaponPrefab;
    public AttackData_SO weaponData;
    public AnimatorOverrideController weaponAnimator;


    [Header("Shield")]
    public GameObject ShieldPrefab;
    public DefenceData_SO shieldData;
    public AnimatorOverrideController shieldAnimator;


    [Header("Usable Item")]

    public UseableItemData_SO useableItemData;
}
