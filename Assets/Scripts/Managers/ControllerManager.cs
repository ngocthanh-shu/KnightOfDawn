using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerManager : Singleton<ControllerManager>
{
    public GameInputActions inputActions;
    public GameInputActions.PlayerActions playerActions;
    public GameInputActions.UIActions uiActions;

    public event Action<Vector2> moveAction;

    public event Action attackAction;
    public event Action attackCancelAction;
    
    public event Action skill1Action;
    public event Action skill2Action;
    public event Action skill3Action;
    public event Action skill4Action;
    public event Action cancelAction;

    
    public void Initialize()
    {
        if (inputActions == null)
            inputActions = new GameInputActions();

        playerActions = inputActions.Player;
        uiActions = inputActions.UI;


        InitializeCallbacks();
        EnableBothActions();
    }

    private void Update()
    {
        moveAction?.Invoke(playerActions.Move.ReadValue<Vector2>());
    }

    private void InitializeCallbacks()
    {
        playerActions.Attack.started += AttackStarted;
        playerActions.Attack.canceled += AttackCanceled;
        
        playerActions.Skill1.started += Skill1Started; 
        playerActions.Skill2.started += Skill2Started;
        playerActions.Skill3.started += Skill3Started;
        playerActions.Skill4.started += Skill4Started;

        uiActions.Cancel.started += CancelStarted;
    }

    #region Main Methods
    public void EnablePlayerActions()
    {
        playerActions.Enable();
        uiActions.Disable();
    }

    public void EnableUIActions()
    {
        playerActions.Disable();
        uiActions.Enable();
    }

    public void EnableBothActions()
    {
        playerActions.Enable();
        uiActions.Enable();
    }

    public void DisableBothActions()
    {
        playerActions.Disable();
        uiActions.Disable();
    }
    #endregion

    #region Callback Methods
    

    private void AttackStarted(InputAction.CallbackContext ctx)
    {
        attackAction?.Invoke();
    }
    
    private void AttackCanceled(InputAction.CallbackContext ctx)
    {
        attackCancelAction?.Invoke();
    }

    private void Skill1Started(InputAction.CallbackContext ctx)
    {
        skill1Action?.Invoke();
    }

    private void Skill2Started(InputAction.CallbackContext ctx)
    {
        skill2Action?.Invoke();
    }

    private void Skill3Started(InputAction.CallbackContext ctx)
    {
        skill3Action?.Invoke();
    }

    private void Skill4Started(InputAction.CallbackContext ctx)
    {
        skill4Action?.Invoke();
    }

    private void CancelStarted(InputAction.CallbackContext ctx)
    {
        cancelAction?.Invoke();
    }
    #endregion
}
