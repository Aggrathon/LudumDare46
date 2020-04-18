using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Blocker))]
public class Unit : MonoBehaviour
{

    public enum Team {
        sheriff,
        bandit,
        neutral
    }

    public Team team = Team.neutral;

    public int speed = 10;
    public int movement = 5;
    [Range(0f, 1f)] public float aggresiveness = 0.5f;

    [System.NonSerialized] public int priority;

    public Health health { get; protected set; }
    public Blocker blocker { get; protected set; }
    public object weapon { get; protected set; }

    private void OnEnable() {
        health = GetComponent<Health>();
        blocker = GetComponent<Blocker>();
        GameManager.activeGM.RegisterUnit(this);
    }

    private void OnDisable() {
        GameManager.activeGM.DeregisterUnit(this);
    }

    public IEnumerator Act(bool inCombat) {
        yield break;
    }
}
