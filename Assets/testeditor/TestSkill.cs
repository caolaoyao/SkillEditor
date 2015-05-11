using UnityEngine;
using System.Collections;

public class TestSkill : MonoBehaviour {
    public int skillId = 2345678;
    private CenterController centerController;
    private SkillController skillController;
    private StatusController statusController;
    private Animator animator; //动画
	// Use this for initialization
	void Start () 
    {
        centerController = gameObject.AddComponent<CenterController>();
        skillController = gameObject.AddComponent<SkillController>();
        statusController = gameObject.AddComponent<StatusController>();
        centerController.skillController = skillController;
        centerController.statusController = statusController;
        animator = gameObject.GetComponent<Animator>();
        centerController.animator = animator;
        statusController.centerController = centerController;
        skillController.centercontroller = centerController;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        if (GUILayout.Button("释放技能"))
        {
            skillController.UseSkillById(skillId);
        }
    }
}
