using Chondria.Entities;

namespace Chondria.Management
{
    public class Scene
    {
        public string Name = "New Scene";

        public Scene() { }
        public Scene(string name)
        {
            Name = name;
        }

        public MeshRenderer[] Objects = [];

        public void Add(MeshRenderer obj)
        {
            var newObjects = new MeshRenderer[Objects.Length + 1];
            for (int i = 0; i < Objects.Length; i++)
            {
                newObjects[i] = Objects[i];
            }
            newObjects[Objects.Length] = obj;
            Objects = newObjects;
        }
    }
}
