using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {
    
    public int health;
    public int tier;
    public int type;
    public bool isActive;
    public int reward;
    
    private MoveEntity movement;

    public void init(int pHealth, int pTier, int pType, bool pIsActive, int pReward) {
        this.health = pHealth;
        this.tier = pTier;
        this.type = pType;
        this.isActive = pIsActive;
        this.reward = pReward;
    }

    void Start() {
        gameObject.AddComponent(typeof(MoveEntity));
        foreach (var component in GetComponents<Component>())
            if (component.GetType().Name == "MoveEntity")
                movement = component as MoveEntity;
    }

    public void setTargets(MapCheckpoints checkpoints) {
        movement.setCheckpoints(checkpoints);
    }

    public void startMoving() {
        //movement.go();
    }
}
