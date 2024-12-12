using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAIBrain : MonoBehaviour, IUnitInput
{
    [field: SerializeField]
    public GameObject Target {  get; set; }
    [field:SerializeField]
    public UnityEvent<Vector2> OnFireButtonPressed { get; set; }
    [field: SerializeField]
    public UnityEvent<Vector2> OnFireButtonReleased { get; set; }
    [field: SerializeField]
    public UnityEvent<Vector2> OnMovementKeyPressed { get; set; }
    [field: SerializeField]
    public UnityEvent<Vector2> OnPointerPositionChange { get; set; }


    private void Start()
    {
        Target = FindAnyObjectByType<PlayerMovement>().gameObject;
    }
}
