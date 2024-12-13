using UnityEngine;

public interface IBuildState
{
    void EndState();
    void OnAction(Vector3Int gridPosition);
    void UpdateState(Vector3Int gridPosition);
}