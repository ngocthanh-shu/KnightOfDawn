using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Purchasing.Extension;

public class EnemyView : MonoBehaviour
{
    [SerializeField] public EnemyType type;
    [SerializeField] public LayerMask layerCanAttack;
    public NavMeshAgent navMeshAgent;
    public AnimatorModule animatorController;
    
    public AudioSource audioSource;
    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animatorController = GetComponent<AnimatorModule>();
        audioSource = GetComponent<AudioSource>();
    }

    public void Initialize(EnemyModel model)
    {
        navMeshAgent.speed = model.baseSpeed;
        navMeshAgent.stoppingDistance = model.attackRange;
    }

    public void MoveToTarget(Vector3 target)
    {
        navMeshAgent.SetDestination(target);

        if (IsInStopDistance())
        {
            animatorController.SetBooleanAttribute("Moving", false);
            return;
        }

        animatorController.SetBooleanAttribute("Moving", true);
    }

    public void RotationToTarget(Vector3 target, float RotationSpeed = 1f)
    {
        Vector3 directionToTarget = target - navMeshAgent.transform.position;
        directionToTarget.y = 0f;
        if(directionToTarget == Vector3.zero) return;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        navMeshAgent.transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, RotationSpeed * Time.deltaTime);
    }
    
    public void SetSpeed(float value)
    {
        navMeshAgent.speed = value;
    }
    
    public bool CanAttack(float attackRange)
    {
        return Physics.CheckSphere(transform.position, attackRange, layerCanAttack);
    }

    public bool IsInStopDistance()
    {
        return (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance);
    }
    
    private void OnEnable()
    {
        InvokeRepeating("PlaySound", UnityEngine.Random.Range(2f, 3f), 2);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void PlaySound()
    {
        AudioManager.Instance.PlayAduioOneShot(audioSource, ResourceManager.Instance.dictionaryAudio["ZombieSound"]);
    }
}


public enum EnemyType
{
    Common,
    Rare,
    Epic
}
