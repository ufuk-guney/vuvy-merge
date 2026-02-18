using System;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer.Unity;

public class GridInputController : IInitializable, IDisposable
{
    private readonly DragHandler _dragHandler;
    private readonly Camera _camera;

    private readonly InputAction _pressAction;
    private readonly InputAction _positionAction;
    private bool _isDragging;

    public GridInputController(DragHandler dragHandler)
    {
        _dragHandler = dragHandler;
        _camera = Camera.main;

        _pressAction = new InputAction("Press", InputActionType.Button, "<Pointer>/press");
        _positionAction = new InputAction("Position", InputActionType.Value, "<Pointer>/position");
    }

    public void Initialize()
    {
        _pressAction.started += OnPressStarted;
        _pressAction.canceled += OnPressCanceled;
        _positionAction.performed += OnPointerMoved;
        _pressAction.Enable();
        _positionAction.Enable();
    }

    private void OnPressStarted(InputAction.CallbackContext ctx)
    {
        _isDragging = false;
        if (ctx.control.device is not Pointer pointer || !pointer.added) return;//written for device simulator input
        if (pointer.position.ReadValue().ScreenToGrid(_camera) is not { } gridPos) return;
        _isDragging = _dragHandler.TryStartDrag(gridPos);
    }

    private void OnPointerMoved(InputAction.CallbackContext ctx)
    {
        if(_isDragging)
        if (ctx.control.device is not Pointer pointer || !pointer.added) return;
        if (ctx.ReadValue<Vector2>().ScreenToWorld(_camera) is not { } worldPos) return;
        _dragHandler.UpdateDragPosition(worldPos);
    }

    private void OnPressCanceled(InputAction.CallbackContext ctx)
    {
        _isDragging = false;
        if (ctx.control.device is not Pointer pointer || !pointer.added) return;
        if (pointer.position.ReadValue().ScreenToGrid(_camera) is not { } dropGridPos) return;
        _dragHandler.EndDrag(dropGridPos);
    }

    public void Dispose()
    {
        _pressAction.started -= OnPressStarted;
        _pressAction.canceled -= OnPressCanceled;
        _positionAction.performed -= OnPointerMoved;
        _pressAction.Disable();
        _positionAction.Disable();
        _pressAction.Dispose();
        _positionAction.Dispose();
    }
}
