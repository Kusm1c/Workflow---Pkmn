using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Pokemon/Create new item")]
public class ItemSO : ScriptableObject
{
    public ItemType itemType;
    public TargetStat targetStat;
    public int effectValue;
    public int ballModifier;

    public string name;
    [TextArea] public string description;
}
