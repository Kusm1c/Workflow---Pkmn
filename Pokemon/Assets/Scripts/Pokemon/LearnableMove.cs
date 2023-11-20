using UnityEngine;

public class LearnableMoveSO : MonoBehaviour
{
    [field: SerializeField] public int Level { get; private set; }
    [field: SerializeField] public MoveSO Move { get; private set; }
}