using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Puppet : MonoBehaviour {

    Animator animator;
    Gyroscope gyroscope;
    public UnityEvent OnDrawWeapon, OnHitTargetEvent, OnFinishAttackEvent, OnFallDeadEvent;

    private void Awake() {
        animator = this.GetComponent<Animator>();
        gyroscope = this.transform.parent.GetComponent<Gyroscope>();
    }

    public void Unselect() {
        animator.SetInteger("interact", 0);
    }
    public void Select() {
        animator.SetInteger("interact", 1);
        if (gyroscope != null) gyroscope.order = 0;
    }
    public void Target() {
        animator.SetInteger("interact", 2);
        if (gyroscope != null) gyroscope.order = -1;
    }

    public void Attacked() {
        animator.SetInteger("action", 2);
        animator.SetTrigger("prepare");
    }
    public void Block() {
        animator.SetTrigger("execute");
        animator.SetInteger("value", 0);
    }
    public void Damage() {
        animator.SetTrigger("execute");
        animator.SetInteger("value", 1);
    }
    public void Die() {
        animator.SetTrigger("execute");
        animator.SetInteger("value", -1);
    }

    public void Evade() {
        animator.SetTrigger("react");
    }

    public void Attack() {
        animator.SetInteger("action", 1);
        animator.SetTrigger("prepare");
    }
    public void ExecuteAction() {
        animator.SetTrigger("execute");
    }
    public void RetreatAction() {
        animator.SetTrigger("back");
    }

    public void Dance() {
        animator.SetTrigger("celebrate");
    }


    public void OnEndAttack() {
        OnFinishAttackEvent.Invoke();
    }
    public void OnDraw() {
        OnDrawWeapon.Invoke();
    }
    public void OnHit() {
        OnHitTargetEvent.Invoke();
    }
    public void OnDead() {
        OnFallDeadEvent.Invoke();
    }
}
