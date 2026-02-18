public abstract class EmergencyState
{
    protected readonly EmergencyStateMachine StateMachine;
    protected readonly EmergencyBehaviour Emergency;
    
    public EmergencyState(EmergencyStateMachine stateMachine)
    {
        StateMachine = stateMachine;
        Emergency = stateMachine.Emergency;
    }
    
    public virtual void Enter() { }

    public virtual void Update() { }

    public virtual void Exit() { }

    public virtual EmergencyState GetNextState() => null;
}