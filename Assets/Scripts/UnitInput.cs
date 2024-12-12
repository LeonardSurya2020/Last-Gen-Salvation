using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnitInput : MonoBehaviour, IUnitInput
{
    [field: SerializeField]
    public UnityEvent<Vector2> OnMovementKeyPressed { get; set; }

    [field: SerializeField]
    public UnityEvent<Vector2> OnPointerPositionChange { get; set; }

    [field: SerializeField]
    public UnityEvent<Vector2> OnFireButtonPressed { get; set; }

    [field: SerializeField]
    public UnityEvent<Vector2> OnFireButtonReleased { get; set; }


}
