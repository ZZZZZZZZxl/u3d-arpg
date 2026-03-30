using UnityEngine;

public class AnimationID
{
    public static readonly int MovementId = Animator.StringToHash("Movement");
    public static readonly int HasInputId = Animator.StringToHash("HasInput");
    public static readonly int DashID = Animator.StringToHash("Dash");
    // public static readonly int StartToRunID = Animator.StringToHash("StartToRun");
    public static readonly int EndToIdleID = Animator.StringToHash("EndToIdle");
    public static readonly int TurnAroundID = Animator.StringToHash("TurnAround");
    public static readonly int IsMovingID = Animator.StringToHash("IsMoving");
    public static readonly int EndDashID = Animator.StringToHash("EndDash");
    
    public static readonly int AttackID = Animator.StringToHash("Attack");
    public static readonly int FinalAttackID = Animator.StringToHash("FinalAttack");
    public static readonly int SpecialAttackID = Animator.StringToHash("SpecialAttack");
    public static readonly int InNormalAttackID = Animator.StringToHash("InNormalAttack");
    public static readonly int TransiteNextAttackID = Animator.StringToHash("TransiteNextAttack");
    public static readonly int InRushingAttackID = Animator.StringToHash("InRushingAttack");
    
    // Enemy
    public static readonly int DieID = Animator.StringToHash("Die");
    public static readonly int HitID = Animator.StringToHash("Hit");
    public static readonly int HeavyID = Animator.StringToHash("Heavy");
    public static readonly int FrontHitID = Animator.StringToHash("FrontHit");
    public static readonly int WalkID = Animator.StringToHash("Walk");
    public static readonly int RunID = Animator.StringToHash("Run");
    public static readonly int BeHittingID = Animator.StringToHash("BeHitting");
    public static readonly int NormalAttackID = Animator.StringToHash("NormalAttack");
    public static readonly int NearAtttackID = Animator.StringToHash("NearAtttack");
    
}