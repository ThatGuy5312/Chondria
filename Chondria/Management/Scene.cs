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

        public List<MeshRenderer> Objects = new();

        public void Add(MeshRenderer obj)
        {
            Objects.Add(obj);
        }
    }
}
