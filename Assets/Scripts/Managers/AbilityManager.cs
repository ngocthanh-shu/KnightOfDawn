using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public Transform skillParent;
    public GameObject normalAttack;
    public GameObject skill1;
    public GameObject skill2;
    public GameObject skill3;
    public GameObject skill4;

    private Skill _normalAttack;
    private Skill _dataSkill1;
    private Skill _dataSkill2;
    private Skill _dataSkill3;
    private Skill _dataSkill4;

    private GameManager _gameManager;
    private PlayerController _playerController;
    
    public void Initialize(GameManager gameManager, PlayerController playerController, List<LevelGetAbility> abilityList)
    {
        if (abilityList.Count > 0)
        {
            GameObject ability;
            foreach(LevelGetAbility levelGetAbility in abilityList)
            {
                ability = Instantiate(levelGetAbility.ability, skillParent.transform.position, Quaternion.identity, skillParent);
                if (normalAttack == null)
                {
                    normalAttack = ability;
                }
                else if(skill1 == null)
                {
                    skill1 = ability;
                }
                else if(skill2 == null)
                {
                    skill2 = ability;
                }
                else if(skill3 == null)
                {
                    skill3 = ability;
                }
                else if(skill4 == null)
                {
                    skill4 = ability;
                }
            }
        }
        
        _gameManager = gameManager;
        _normalAttack = normalAttack.GetComponent<Skill>();
        _dataSkill1 = skill1.GetComponent<Skill>();
        _dataSkill2 = skill2.GetComponent<Skill>();
        _dataSkill3 = skill3.GetComponent<Skill>();
        _dataSkill4 = skill4.GetComponent<Skill>();
        
        _playerController = playerController;
    }

    public void NormalAttack()
    {
        if (_normalAttack.CooldownCalculate())
        {
            SetPosition(normalAttack, GetTransformPlayer().position);
            _normalAttack.Active(null,_playerController, ResourceManager.Instance.dictionaryAudio["Slash"]);
        }
    }
    
    public void Skill1()
    {
        if (_dataSkill1.CooldownCalculate())
        {
            Vector3 position = GetTransformPlayer().position + GetTransformPlayer().forward * -1.5f;
            SetPosition(skill1, position);
            GameObject enemy = _dataSkill1.GetNearestEnemy(_playerController.model.enemyList);
            if (enemy != null)
            {
                _playerController.view.StartCoroutine(_playerController.StayRotate(enemy));
            }
            SetRotation(skill1, GetTransformPlayer().rotation.eulerAngles);
            _dataSkill1.Active(enemy,_playerController,ResourceManager.Instance.dictionaryAudio["RocketFire"]);
        }
    }
    
    public void Skill2()
    {
        if(_dataSkill2.CooldownCalculate())
        {
            SetPosition(skill2, GetTransformPlayer().position);
            _dataSkill2.Active(null,_playerController, ResourceManager.Instance.dictionaryAudio["PuttingBomb"]);
        }
    }
    
    public void Skill3()
    {
        if(_dataSkill3.CooldownCalculate())
        {
            SetPosition(skill3, GetTransformPlayer().position);
            _dataSkill3.Active(null,_playerController);
        }
    }
    
    public void Skill4()
    {
        if(_dataSkill4.CooldownCalculate())
        {
            SetPosition(skill4, GetTransformPlayer().position);
            GameObject enemy = _dataSkill4.GetNearestEnemy(_playerController.model.enemyList);
            if (enemy != null)
            {
                _playerController.view.StartCoroutine(_playerController.StayRotate(enemy));
            }
            _dataSkill4.Active(enemy,_playerController, ResourceManager.Instance.dictionaryAudio["BlackHole"]);
        }
    }
    
    private void SetPosition(GameObject skill, Vector3 position)
    {
        skill.transform.position = position;
    }
    
    private void SetRotation(GameObject skill, Vector3 rotation)
    {
        skill.transform.rotation = Quaternion.Euler(rotation);
    }

    private Transform GetTransformPlayer()
    {
        return _playerController.view.transform;
    }
}
