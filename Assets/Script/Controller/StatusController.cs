using UnityEngine;
using System.Collections;

public class StatusController : MonoBehaviour {

	// Use this for initialization

    public Status curStatus = Status.Idle; //当前的状态
    public CenterController centerController; //所有controller集合
    private Animator _animator; //动画
	void Start () 
    {
        _animator = centerController.animator;
	}

    //播放动画
    public void Play(Status status)
    {
        curStatus = status;
        _animator.SetInteger("Status", (int)status);
        _animator.Play(status.ToString(), -1, 0f);
    }

    //获取动画时间
    public float GetNormalizedTime()
    {
        return _animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }

    //判断状态机是否跟动画一致
    public bool IsSameStatus()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName(curStatus.ToString()))
        {
            return true;
        }
        return false;
    }
}
