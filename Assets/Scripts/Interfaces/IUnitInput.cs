using UnityEngine;
using UnityEngine.Events;

public interface IUnitInput
{
    UnityEvent<Vector2> OnFireButtonPressed { get; set; }
    UnityEvent<Vector2> OnFireButtonReleased { get; set; }
    UnityEvent<Vector2> OnMovementKeyPressed { get; set; }
    UnityEvent<Vector2> OnPointerPositionChange { get; set; }
}