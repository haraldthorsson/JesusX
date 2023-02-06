using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerPreset", menuName = "ScriptableObjects/PlayerPreset")]
public class PlayerPreset : ScriptableObject
{
    public float walkSpeed;
    public float jumpSpeed;

    public float MaxHP;
    public float damage;

    public AnimationClip idle;
    public AnimationClip walkRight;
    public AnimationClip walkLeft;
    public AnimationClip Attack;
    public AnimationClip AttackUp;
    public AnimationClip AttackDown;
    public AnimationClip Kick;
    public AnimationClip KickDown;
    public AnimationClip airAttack;

    public AnimationClip block;
    public AnimationClip blockUp;
    public AnimationClip blockDown;

    public AnimationClip Jump;
    public AnimationClip JumpDown;
    
    public AnimationClip intro;
    public AnimationClip death;
    public AnimationClip hit;
    public AnimationClip hitAir;


}
