using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator _animator;
    private bool _isAttacking;
    private bool _isMoving;
    private float horTmp;
    private float verTmp;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        horTmp = 0;
        verTmp = 0;
    }

    public void SetMovementAnimation(float horizontal, float vertical)
    {
        if (_isMoving && !_isAttacking)
        {
            horizontal = Mathf.Abs(horizontal);
            vertical = Mathf.Abs(vertical);
        }

        _animator.SetFloat("Horizontal", horizontal + horTmp);
        _animator.SetFloat("Vertical", vertical + verTmp);
    }
    
    public void SetMovingAnimation(bool isMoving)
    {
        _animator.SetBool("IsMoving", isMoving);
        _isMoving = isMoving;
    }
    
    public bool GetMovingAnimation()
    {
        return _isMoving;
    }

    public void SetAttackAnimation(bool isAttacking)
    {
        _animator.SetBool("IsAttacking", isAttacking);
        _isAttacking = isAttacking;
        bool isAttack = _animator.GetCurrentAnimatorStateInfo(1).IsName("Attacking");
        SetLayer(isAttack);
    }
    
    public bool GetAttackAnimation()
    {
        return _isAttacking;
    }
    
    public void CastingSkillAnimation()
    {
        _animator.SetTrigger("Casting");
    }

    public void SetDirection(float horizontal, float vertical)
    {
        if(horizontal == 0 && vertical == 0) return;
        horTmp = -horizontal;
        verTmp = 1 - vertical;
    }
    
    public void ResetDirection()
    {
        horTmp = 0;
        verTmp = 0;
    }
    
    //get current animation state
    public AnimatorStateInfo GetCurrentAnimatorStateInfo()
    {
        return _animator.GetCurrentAnimatorStateInfo(0);
    }

    public void SetLayer(bool isAttacking)
    {
        _animator.SetLayerWeight(1, isAttacking ? 1 : 0);
    }
    
    public Animator GetAnimator()
    {
        return _animator;
    }
}
