using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Enemies/EnemyData")]
public class EnemyDataSO : ScriptableObject
{
    [field: SerializeField]
    public int maxHealth { get; set; } = 3;
    [field: SerializeField]
    public int damage { get; set; } = 2;
    [field: SerializeField]
    public int speed { get; set; } = 1;

}
