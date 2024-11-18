namespace Syncra;

/// <summary>
/// Represents a system in the ECS.
/// </summary>
public interface ISystem
{
    /// <summary>
    /// The main hook for running the system.
    /// </summary>
    public static void Update()
    {
    }
}