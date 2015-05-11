using UnityEngine;
using System.Collections;

public class SkillController : MonoBehaviour {
    public CenterController centercontroller;
    private Skill _curSkill; //当前使用的技能
	// Use this for initialization
	void Start () 
    {	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (_curSkill == null)
        {
            return;
        }

        _curSkill.Update();
	}

    //根据id使用技能
    public bool UseSkillById(int skillId)
    {
        _curSkill = SkillManager.instance.CreateSkill(skillId);
        _curSkill.centerController = centercontroller;
        if (_curSkill == null)
        {
            return false;
        }
        _curSkill.Use();
        return true;
    }
}
