public interface WeaponControlsInterface
{
    // primary weapon has been fired.
    bool PrimaryFired { get; }

    // secondary weapon has been fired.
    bool SecondaryFired { get; }
}