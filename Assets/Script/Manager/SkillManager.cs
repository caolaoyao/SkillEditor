using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BT.Util;

public class SkillManager : Singleton<SkillManager> 
{
    //缓存的技能
    private Dictionary<int, Skill> _skillDisc= new Dictionary<int, Skill>();

    //创建技能
    public Skill CreateSkill(int skillId)
    {
        //如果有缓存，直接返回，无需再创建
        if (_skillDisc.ContainsKey(skillId))
        {
            return _skillDisc[skillId];
        }

        //加载技能数据
        Skill skill = Resources.Load("FightData/Skill/" + skillId+"/Skill") as Skill;
        if (skill != null)
        {
            //缓存
            _skillDisc[skillId] = skill;
        }
        return skill;
    }
}
