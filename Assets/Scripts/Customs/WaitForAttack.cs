using UnityEngine;
using System.Collections;

public class WaitForAttack : CustomYieldInstruction {
    private bool isAttackFinished = false;

    public override bool keepWaiting {
        get { return !isAttackFinished; }
    }

    public void setAttackFinished() {
        isAttackFinished = true;
    }
}