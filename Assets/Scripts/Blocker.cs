using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Blocker : MonoBehaviour
{
    public float moveHeight = 0.3f;
    public float moveSpeed = 2f;

    PathGrid grid;
    int posi, posj;
    List<Vector3> moves;

    private void OnEnable() {
        moves = new List<Vector3>();
        grid = PathGrid.activeGrid;
        (posi, posj) = grid.VectorToIndex(transform.position);
        grid.MoveInto(posi, posj);
    }

    private void OnDisable() {
        grid.MoveOut(posi, posj);
    }

    public bool Move(int i, int j) {
        if (grid.CanMoveInto(i, j)) {
            grid.MoveOut(posi, posj);
            grid.MoveInto(i, j);
            posi = i;
            posj = j;
            transform.position = grid.IndexToVector(i, j);
            return true;
        } else {
            return false;
        }
    }

    public bool Move(Vector3 target) {
        (int i, int j) = grid.VectorToIndex(target);
        return Move(i, j);
    }

    public bool MoveAnimated(List<Vector3> moves, int i, int j) {
        if (grid.CanMoveInto(i, j)) {
            grid.MoveOut(posi, posj);
            grid.MoveInto(i, j);
            posi = i;
            posj = j;
            this.moves = moves;
            StartCoroutine(MoveAnimation());
            return true;
        } else {
            return false;
        }
    }

    public bool MoveAnimated(List<Vector3> moves) {
        (int i, int j) = grid.VectorToIndex(moves[moves.Count - 1]);
        return MoveAnimated(moves, i, j);
    }

    IEnumerator MoveAnimation() {
        if (moves.Count == 0)
            yield break;
        Vector3 vel = Vector3.zero;
        Vector3 prev = transform.position;
        int i = 0;
        while (i < moves.Count) {
            Vector3 move = moves[i];
            float d2 = Vector3.SqrMagnitude(transform.position - move);
            if (d2 < 0.01f) {
                prev = moves[i];
                i++;
                continue;
            }
            move.y = transform.position.y;
            move = Vector3.SmoothDamp(transform.position, move, ref vel, 0.3f/moveSpeed, moveSpeed);
            float d1 = Vector3.SqrMagnitude(transform.position - prev);
            move.y = (1f - (d1 + d2 - 0.5f) * 2f) * moveHeight;
            transform.position = move;
            yield return null;
        }
        if (moves.Count > 0)
            transform.position = moves[moves.Count - 1];
    }

    public void RandomMove(int maxSteps = 2) {
        var paths = grid.GetReachable(posi, posj, maxSteps);
        var target = paths.ElementAt(Random.Range(0, paths.Count)).Value;
        moves.Clear();
        for (int i = 0; i < target.d + 1; i++)
        {
            moves.Add(Vector3.zero);
        }
        int id = target.id;
        while (id != -1) {
            var node = paths[id];
            moves[node.d] = grid.IndexToVector(node.i, node.j);
            id = node.pid;
        }
        MoveAnimated(moves, target.i, target.j);
    }
    
    [ContextMenu("Random Move 2")] private void RandomMove2() { RandomMove(2); }
    [ContextMenu("Random Move 5")] private void RandomMove5() { RandomMove(5); }
}
