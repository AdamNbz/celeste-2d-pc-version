using UnityEngine;

public interface State
{
    void Enter();
    void FixedUpdate();
    void Update();
    void Exit();
}
