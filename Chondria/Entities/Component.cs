namespace Chondria.Entities;

// The core component class that turns a class into a working scriptable component for a Entity
public abstract class Component : Core.Object
{
    public Entity? Entity;

    public Transform? Transform => Entity?.Transform;


    // i was going to do reflection to find the methods using these names but that a lot slower.
    public virtual void Awake() { }

    public virtual void Start() { }

    public virtual void Update() { }

    internal virtual void OnEditorGui() { }

    public virtual void OnDestroy() { }

    public virtual void OnEnable() { }

    public virtual void OnDisable() { }
}
