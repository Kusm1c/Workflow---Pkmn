using UnityEngine;

[CreateAssetMenu(fileName = "Move", menuName = "Pokemon/Create new Move")]
public class MoveSO : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public int ID { get; private set; }
    [field: SerializeField] public PokemonType Type { get; private set; }
    [field: SerializeField] public MoveType MoveType { get; private set; }
    [field: SerializeField] public int PP { get; set; }
    [field: SerializeField] public int MaxPP { get; private set; }
    [field: SerializeField] public int Power { get; private set; }
    [field: SerializeField] public int Accuracy { get; private set; }
    [field: SerializeField] public int Priority { get; private set; }
    [field: SerializeField] public int CritRatio { get; private set; }
    [field: SerializeField] public StatusEffect StatusEffect { get; private set; }
    [field: SerializeField] public Buff Buff { get; private set; }
    [field: SerializeField] public Target Target { get; private set; }
    
}