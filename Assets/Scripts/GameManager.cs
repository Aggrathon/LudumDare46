﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum State {
        Stopped,
        Setup,
        Starting,
        PreCombat,
        Combat
    }

    public static GameManager activeGM { get; protected set; }
    
    List<Unit> units;
    State state;


    public GameObject winScreen;
    public GameObject looseScreen;
    public GameObject spottedScreen;

    private void OnEnable() {
        activeGM = this;
        units = new List<Unit>();
        state = State.Setup;
    }

    public void RegisterUnit(Unit unit) {
        if (state == State.Setup && unit.team == Unit.Team.bandit) {
            state = State.PreCombat;
            StartCoroutine(StateMachine());
        }
        unit.priority = Random.Range(1, unit.speed);
        units.Add(unit);
    }

    public void DeregisterUnit(Unit unit) {
        units.Remove(unit);
        foreach(var u in units)
            if (u.team == unit.team)
                return;
        NotifyLost(unit.team);
    }

    public IEnumerable<Unit>EnumerateEnemies(Unit.Team team) {
        if (team == Unit.Team.neutral)
            yield break;
        foreach(var u in units)
            if (team != u.team)
                yield return u;
    }

    private Unit GetNextUnit() {
        units.Sort((a, b)=> a.priority.CompareTo(b.priority));
        if (units.Count > 0) {
            int minpri = units[0].priority;
            foreach(var u in units) {
                u.priority += minpri;
            }
            units[0].priority += units[0].speed;
            UpdateTurnOrderVisual();
            return units[0];
        }
        return null;
    }

    private void UpdateTurnOrderVisual() {

    }

    private IEnumerator StateMachine() {
        yield return null;
        Unit unit;
        while(true) {
            switch (state)
            {
                case State.Starting:
                    yield return new WaitForSeconds(1f);
                    state = State.PreCombat;
                    break;
                case State.PreCombat:
                    unit = GetNextUnit();
                    yield return unit.Act(false);
                    break;
                case State.Combat:
                    unit = GetNextUnit();
                    yield return unit.Act(true);
                    break;
                default:
                    yield break;
            }
        }
    }

    public void NotifyCombat() {
        if (state == State.PreCombat) {
            state = State.Combat;
            spottedScreen.SetActive(true);
        }
    }

    public void NotifyLost(Unit.Team team) {
        if (team == Unit.Team.sheriff) {
            winScreen.SetActive(true);
            state = State.Stopped;
            StopAllCoroutines();
        } else if (team == Unit.Team.bandit) {
            looseScreen.SetActive(true);
            state = State.Stopped;
            StopAllCoroutines();
        }
    }
}
