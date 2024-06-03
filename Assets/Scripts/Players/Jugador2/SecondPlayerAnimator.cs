using UnityEngine;

public class SecondPlayerAnimator : IAnimate
{
    Animator animator;

    public SecondPlayerAnimator(Animator animator)
    {
        this.animator = animator;
    }


    int CurrentAttack { get; set; } = 1;
    public void Attack()
    {
        animator.SetTrigger(SecondPlayerAnimations.attack + CurrentAttack);
    }

    public void Block()
    {
        animator.SetTrigger(SecondPlayerAnimations.block);
    }

    public void Climb()
    {
        throw new System.NotImplementedException();
    }

    public void Die(bool isShowingBlood)
    {
        animator.SetBool(SecondPlayerAnimations.noBlood, isShowingBlood);
        animator.SetTrigger(SecondPlayerAnimations.death);
    }

    public void Fall(bool isGrounded)
    {
        animator.SetBool(SecondPlayerAnimations.grounded, isGrounded);
    }

    public void Hurt()
    {
        animator.SetTrigger(SecondPlayerAnimations.hurt);
    }

    public void Idle()
    {
        animator.SetInteger(SecondPlayerAnimations.animState, 0);
    }

    public void IdleBlock(bool isActivated)
    {
        animator.SetBool(SecondPlayerAnimations.idleBlock, isActivated);
    }

    public void Jump()
    {
        animator.SetTrigger(SecondPlayerAnimations.jump);
    }

    public void Roll()
    {
        animator.SetTrigger(SecondPlayerAnimations.roll);
    }

    public void Run()
    {
        animator.SetInteger(SecondPlayerAnimations.animState, 1);
    }

    public void WallSlide(bool isSliding)
    {
        animator.SetBool(SecondPlayerAnimations.wallSlide, isSliding);
    }
}