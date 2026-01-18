using UnityEngine;

public interface State
{
    string GetStateName();
    void Enter();
    void FixedUpdate();
    void Update();
    void Exit();
}
