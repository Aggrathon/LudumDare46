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
    public Color color = new Color(1f, 0.8f, 0.5f);

    [SerializeField] Renderer[] coloredRenderers;

    [System.NonSerialized] public int energy;
    [System.NonSerialized] public int priority;

    public Health health { get; protected set; }
    public Blocker blocker { get; protected set; }
    public object weapon { get; protected set; }

    private void OnEnable() {
        health = GetComponent<Health>();
        blocker = GetComponent<Blocker>();
        GameManager.activeGM.RegisterUnit(this);
        foreach(var r in coloredRenderers)
            r.material.color = color;
    }

    private void OnDisable() {
        GameManager.activeGM.DeregisterUnit(this);
    }

    public IEnumerator Act(bool inCombat) {
        CameraController.active.FocusOn(transform);
        energy = actions;
        if (team == Team.bandit) {
            UnitUI.active.Show(this);
            while (energy > 0)
                yield return null;
            UnitUI.active.Hide();
        } else if (team == Team.sheriff) {
            if (!inCombat) {
                foreach(Unit u in GameManager.activeGM.EnumerateEnemies(team)) {
                    RaycastHit hit;
                    Vector3 dir = ( u.transform.position - transform.position).normalized;
                    if(Physics.Raycast(transform.position + dir * 0.5f + transform.up * 1.5f, dir, out hit, sight)) {
                        if (hit.rigidbody?.gameObject == u.gameObject) {
                            GameManager.activeGM.NotifyCombat();
                            inCombat = true;
                            break;
                        }
                    }
                }
            }
            yield return null;
            if (inCombat) {
                //TODO: Move
                energy = -1;
                yield return new WaitForSeconds(0.5f);
                while(energy > 0)
                    yield return null;
                //TODO: Shoot
            }
        } else if (team == Team.leader) {
            //TODO: maybe start hanging at some point?
            //TODO: maybe join the bandits at some point?
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + Vector3.up*0.01f, sight);
    }
}
