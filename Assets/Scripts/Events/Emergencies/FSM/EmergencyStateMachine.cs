using UnityEngine;

public class EmergencyStateMachine
{
    public EmergencyBehaviour Emergency { get; private set; }
    
    private EmergencyState _currentState;
    public readonly EmergencyExpiringState ExpiringState;
    public readonly EmergencySolvingState SolvingState;

    public EmergencyStateMachine(EmergencyBehaviour emergency)
    {
        this.Emergency = emergency;
        ExpiringState = new EmergencyExpiringState(this);
        SolvingState = new EmergencySolvingState(this);
        _currentState = ExpiringState;
    }

    public void Update()
    {
        _currentState.Update();
        
        EmergencyState nextState = _currentState.GetNextState();
        if (nextState != null && nextState != _currentState)
        {
            ChangeState(nextState);
        }
    }

    private void ChangeState(EmergencyState newState)
    {
        if (_currentState != null)
        {
            _currentState.Exit();
        }
        
        _currentState = newState;
        _currentState.Enter();
    }
}