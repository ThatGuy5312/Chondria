using Chondria.Entities;

namespace Chondria.Management
{
    // Main scene class that holds world data
    public class Scene
    {
        public static Scene CurrentScene { get; internal set; }

        public static void LoadScene(Scene scene)
        {
            CurrentScene = scene;
        }

        public string Name = "New Scene";

        public Scene() { }
        public Scene(string name)
        {
            Name = name;
        }

        List<Entity> m_Entities = [];

        public Entity[] Objects => [.. m_Entities]; // what are these "simplifications" bro

        public void AddEntity(Entity entity)
        {
            if (!m_Entities.Contains(entity))
                m_Entities.Add(entity);
        }

        public void RemoveEntity(Entity entity)
        {
            if (m_Entities.Contains(entity))
                m_Entities.Remove(entity);
        }

        public static void AddEntityToLoadedScene(Entity entity)
        {
            if (CurrentScene == null)
            {
                Console.WriteLine("No scene loaded, cannot add entity.");
                return;
            }

            if (!CurrentScene.m_Entities.Contains(entity))
                CurrentScene.m_Entities.Add(entity);
        }

        public static void RemoveEntityFromLoadedScene(Entity entity)
        {
            if (CurrentScene == null)
            {
                Console.WriteLine("No scene loaded, cannot add entity.");
                return;
            }

            if (CurrentScene.m_Entities.Contains(entity))
                CurrentScene.m_Entities.Remove(entity);
        }
    }
}
