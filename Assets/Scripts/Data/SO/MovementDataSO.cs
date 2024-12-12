using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/UnitData")]
public class MovementDataSO : ScriptableObject
{
    [field: SerializeField]
    public int speed { get; set; } = 3;

    [field: SerializeField]
    public int maxHealth { get; set; } = 3;

    [field: SerializeField]
    public int Damage { get; set; } = 3;
}
