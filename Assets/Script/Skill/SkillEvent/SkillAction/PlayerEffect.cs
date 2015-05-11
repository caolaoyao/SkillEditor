using UnityEngine;
using System.Collections;

public class PlayerEffect : SkillAction
{
    public string effectName = "";
    private GameObject effectGo = null;
    public PlayerEffect()
        :base()
    {
    }

    public override void Execute()
    {
        GameObject effect = Resources.Load("Effect/" + effectName) as GameObject;
        if (effect != null)
        {
//             effect.transform.parent = _skill.centerController.gameObject.transform;
            effectGo = GameObject.Instantiate(effect, Vector3.zero, Quaternion.identity) as GameObject;
        }
    }

    public override void Finish()
    {
        if (effectGo != null)
        {
            Destroy(effectGo);
        }
    }
}
