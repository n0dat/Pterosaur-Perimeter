using UnityEngine;

// custom class to ensure a tower attack has finished its animation / spawning of laser
// before next attack happens to next eneny
public class WaitForAttack : CustomYieldInstruction {
    private bool isAttackFinished = false;

    public override bool keepWaiting {
        get { return !isAttackFinished; }
    }
    // Sets the value of isAttackFinsished to true
    public void setAttackFinished() {
        isAttackFinished = true;
    }
}