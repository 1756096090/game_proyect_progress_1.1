using System;
using UnityEngine;

public interface IAnimate
{
    public void Run();
    public void Climb();
    public void Idle();
    public void Jump();
    public void Fall(bool isGrounded);
    public void Roll();
    public void Attack();
    public void Die(bool isShowingBlood);
    public void Hurt();
    public void Block();
    public void IdleBlock(bool isActivated);
    public void WallSlide(bool isSliding);

}