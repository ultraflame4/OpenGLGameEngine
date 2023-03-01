using OpenGLGameEngine.Components;

namespace OpenGLGameEngine;

/// <summary>
///     Many entities will have a transform component and a script component.
///     EntityObject is a convenience class that combines these two components and allows creation of such entities to be easier.
/// </summary>
public abstract class EntityObject : EntityScript
{
    public Transform transform;
}