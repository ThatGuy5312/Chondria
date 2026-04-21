using Chondria.Management;
using Chondria.Rendering;

namespace Chondria.Entities;

// been waiting to make this
// Main entity/gameobject class that holds a Name, Transform, and a List of Components
public class Entity : Core.Object
{
    public Transform Transform { get; set; }

    List<Component> m_Components = [];

    public Entity(string name)
    {
        Name = name;

        var transform = AddComponent<Transform>();
        Transform = transform;
    }

    public Entity(string name, Transform transform)
    {
        Name = name;

        Transform = transform;
    }

    public Entity(string name, params Component[] components)
    {
        Name = name;

        var transform = AddComponent<Transform>();
        Transform = transform;

        foreach (var component in components)
            AddComponent(component);
    }

    public Entity(string name, Transform transform, params Component[] components)
    {
        Name = name;

        Transform = transform;

        foreach (var component in components)
            AddComponent(component);
    }

    public static Entity Create(string name, BaseMesh mesh)
    {
        var entity = new Entity(name);
        var meshComp = entity.AddComponent<MeshRenderer>();
        meshComp.Mesh = new Mesh(BaseMeshes.BASE_MESHES[mesh]);
        Scene.AddEntityToLoadedScene(entity);
        return entity;
    }
    public static Entity Create(string name, BaseMesh mesh, Transform transform)
    {
        var entity = new Entity(name, transform);
        var meshComp = entity.AddComponent<MeshRenderer>();
        meshComp.Mesh = new Mesh(BaseMeshes.BASE_MESHES[mesh]);
        Scene.AddEntityToLoadedScene(entity);
        return entity;
    }

    public Component AddComponent(Component comp)
    {
        comp.Entity = this;
        m_Components.Add(comp);
        comp.Awake();
        return comp;
    }

    public Component AddComponent(Type comp)
    {
        var compInstance = (Component)Activator.CreateInstance(comp);
        compInstance.Entity = this;
        m_Components.Add(compInstance);
        compInstance.Start();
        return compInstance;
    }

    public T AddComponent<T>() where T : Component, new()
    {
        T comp = new();
        AddComponent(comp);
        return comp;
    }

    public T GetComponent<T>() where T : Component =>
        m_Components.OfType<T>().FirstOrDefault();

    public Component GetComponent(Type type) =>
        m_Components.FirstOrDefault(c => c.GetType() == type);

    public IEnumerable<Component> GetAllComponents() => m_Components;
    public IEnumerable<T> GetComponents<T>() => m_Components.OfType<T>();

    public void RemoveComponent<T>() where T : Component
    {
        var comp = GetComponent<T>();
        if (comp != null)
        {
            m_Components.Remove(comp);
            //Destroy(comp);
        }
    }

    public void RemoveComponent(Component comp)
    {
        if (m_Components.Contains(comp))
        {
            m_Components.Remove(comp);
            //Destroy(comp);
        }
    }

    public bool TryGetComponent<T>(out T component) where T : Component
    {
        component = GetComponent<T>();
        return component != null;
    }

    public T AddOrGetComponent<T>() where T : Component, new()
    {
        var comp = GetComponent<T>();
        comp ??= AddComponent<T>();
        return comp;
    }
}
