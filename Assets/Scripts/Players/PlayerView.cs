using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private float horizontal;
    private float vertical;
    private Camera _camera;
    public LayerMask enemyLayer;
    private GameObject _enemyTarget;
    private PlayerAnimation playerAnimation;
    private AudioSource _audioSource;

    public void SetCamera(Camera camera)
    {
        _camera = camera;
        _audioSource = GetComponent<AudioSource>();
    }

    public void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        playerAnimation = GetComponent<PlayerAnimation>();
    }

    public PlayerAnimation Animation()
    {
        return playerAnimation;
    }
    
    public AudioSource AudioSource()
    {
        return _audioSource;
    }
    
    public void Move(float speed)
    {
        MovementWithouAttack(speed);
    }

    public void MovementWithouAttack(float speed)
    {
        Movement(speed);
    }

    public void Movement(float speed)
    {
        Vector3 movement = new Vector3(horizontal, 0, vertical).normalized;
        float cameraRotationY = _camera.transform.eulerAngles.y;
        movement = Quaternion.Euler(0, cameraRotationY, 0) * movement;
        _rigidbody.MovePosition(transform.position + movement * (speed * Time.deltaTime));
    }

    public void Rotate(bool isAttacking,float vision, List<GameObject> enemies, GameObject rightTarget = null)
    {
        if (rightTarget == null)
        {
            if (isAttacking)
            {
                GameObject target = DetectNearestEnemy(vision, enemies);
                if (target != null)
                {
                    RotateToEnemy(target);
                }
            }
            else
            {
                RotateByInput();
            }
        }
        else
        {
            RotateToEnemy(rightTarget);
        }
    }
    
    public void RotateByInput()
    {
        //rotate by input isometric
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;
        if (direction.magnitude >= 0.1f)
        {
            float cameraRotationY = _camera.transform.eulerAngles.y;
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraRotationY;
            var rigidbodyVelocity = _rigidbody.velocity;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rigidbodyVelocity.y, 0.01f);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
    }
    
    public void RotateToEnemy(GameObject enemy)
    {
        Vector3 direction = enemy.transform.position - transform.position;
        direction.y = 0;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 1f);
    }

    public GameObject DetectNearestEnemy(float vision, List<GameObject> enemies)
    {
        GameObject nearestEnemy = null;
        if (_enemyTarget != null && _enemyTarget.activeSelf)
        {
            float distance = Vector3.Distance(transform.position, _enemyTarget.transform.position);
            if (distance <= 3f)
            {
                return _enemyTarget;
            }
        }
        else
        {
            GameObject[] ememies = Array.FindAll(enemies.ToArray(), enemy => Vector3.Distance(transform.position, enemy.transform.position) <= vision && enemy.activeSelf);
            float nearestDistance = Mathf.Infinity;
            foreach (var enemy in ememies)
            {
                IDamage enemyDamage = enemy.GetComponent<IDamage>();
                if (enemyDamage != null)
                {
                    float distance = Vector3.Distance(transform.position, enemy.transform.position);
                    if (distance < nearestDistance && enemy.gameObject != null)
                    {
                        nearestDistance = distance;
                        nearestEnemy = enemy.gameObject;
                    }
                }
            }
        }
        _enemyTarget = nearestEnemy;
        return _enemyTarget;
    }

    public void SetDirection(Vector2 direction)
    {
        horizontal = direction.x;
        vertical = direction.y;
    }
    
    public float GetDirectionX()
    {
        return horizontal;
    }
    
    public float GetDirectionY()
    {
        return vertical;
    }
    
    public bool IsMoving()
    {
        return horizontal != 0 || vertical != 0;
    }
}
