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
        neutral,
        leader
    }

    public Team team = Team.neutral;

    public int speed = 10;
    public int movement = 5;
    [Range(0f, 1f)] public float aggresiveness = 0.5f;
    public int actions = 2;
    public float sight = 10f;

    [System.NonSerialized] public int energy;
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
        if (team == Team.bandit) {
            UnitUI.active.Show(this);
            energy = actions;
            while (energy > 0)
                yield return null;
            UnitUI.active.gameObject.SetActive(false);
        } else if (team == Team.sheriff) {
            if (!inCombat) {
                foreach(Unit u in GameManager.activeGM.EnumerateEnemies(team)) {
                    RaycastHit hit;
                    Vector3 dir = ( u.transform.position - transform.position).normalized;
                    if(Physics.Raycast(transform.position + dir * 0.5f + transform.up * 1.8f, dir, out hit, sight)) {
                        if (hit.collider.gameObject == u.gameObject) {
                            GameManager.activeGM.NotifyCombat();
                            inCombat = true;
                            break;
                        }
                    }
                }
            }
            yield return null;
            if (inCombat) {
                //TODO: Move and Shoot
            }
        } else if (team == Team.leader) {
            //TODO: maybe start hanging at some point?
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + Vector3.up*0.01f, sight);
    }
}
