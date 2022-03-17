using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerInfo : MonoBehaviour
{
    public GridImmobileEntity Entity
    {
        get { return _entity; }
        set
        {
            _entity = value;
            UpdateInfo();
        }
    }
    private GridImmobileEntity _entity;
    [SerializeField]
    private Image _towerImage;
    [SerializeField]
    private TextMeshProUGUI _towerName;
    [SerializeField]
    private TextMeshProUGUI _towerSpecs;

    private void UpdateInfo()
    {
        
        if(_entity is BaseTower)
        {
            BaseTower tower = (BaseTower)_entity;
            ScriptableTower scriptableTower = tower.TowerBlueprint;
            _towerImage.sprite = scriptableTower.sprite;
            _towerName.text = scriptableTower.name;
            _towerSpecs.text = "";
            _towerSpecs.text += $"Attack Damage: {scriptableTower.BaseStats.damage}\n" +
                $"Attack Range: {scriptableTower.BaseStats.range}\n";
            if (scriptableTower.BaseAuras.trueSight)
            {
                _towerSpecs.text += $"Special: This Tower can spot invisible enemy";
            }
        }
        else if (_entity is Stone)
        {
            Stone stone = (Stone)_entity;
            _towerName.text = "Stone";
            _towerImage.sprite = stone.Image;
            _towerSpecs.text = "";
        }
        
        
    }

}
