using UnityEngine;
using Zenject;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    public void MeleeAttackAnimate()
    {
        _animator.SetTrigger("Punch");
    }
    public void RangeAttackAnimate()
    {
        _animator.SetTrigger("Shoot");
    }
    public void DeadAnimate()
    {
        _animator.SetTrigger("Dying");
    }
}
