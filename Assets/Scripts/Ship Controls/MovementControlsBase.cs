

public abstract class MovementControlsBase : MovementControlsInterface
{
    public abstract float ThrustAmount { get; }
    public abstract float YawAmount { get; }
    public abstract float PitchAmount { get; }
    public abstract float RollAmount { get; }

   
}
