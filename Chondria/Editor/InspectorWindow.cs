using Chondria.Entities;
using Chondria.Math;
using Chondria.Rendering;
using Chondria.Windowing;
using ImGuiNET;
using System.Reflection;

namespace Chondria.Editor
{
    [EditorWindow("Inspector")]
    internal class InspectorWindow : EditorWindow
    {
        [EditorWindowDraw]
        public void Draw()
        {
            /*if (ImGui.CollapsingHeader("Lighting Settings"))
            {
                for (int i = 0; i < LightingSettings.Lights.Count; i++)
                {
                    var light = LightingSettings.Lights[i];

                    LightingSettings.Lights[i] = DrawLight(light, i);
                }
            }

            if (ImGui.BeginPopupContextWindow("Create Popup", ImGuiPopupFlags.MouseButtonRight))
            {
                ImGui.Text("CREATE");

                ImGui.Separator();

                if (ImGui.MenuItem("Add Light"))
                {
                    LightingSettings.Lights.Add(new Light
                    {
                        Position = new(0, 0, 0),
                        Color = new(1, 1, 1),
                        Linear = 0.09f,
                        Constant = 1f,
                        Quadratic = 0.032f
                    });
                }
                ImGui.EndPopup();
            }*/

            if (Inspector.NullSelection)
            {
                ImGui.Text("No Entity Selected");
                return;
            }

            DrawEntity(ref Inspector.Selected);
        }

        Light DrawLight(Light light, int index)
        {
            var numLightColor = light.Color.Numerics();
            if (ImGui.ColorEdit3($"Light {index} Color", ref numLightColor))
                light.Color = numLightColor;

            ImGui.DragFloat($"Light {index} Linear", ref light.Linear, 0.01f);

            ImGui.DragFloat($"Light {index} Constant", ref light.Constant, 0.1f);

            ImGui.DragFloat($"Light {index} Quadratic", ref light.Quadratic, 0.01f);

            return light;
        }

        void DrawEntity(ref Entity entity)
        {
            ImGui.Text("Name: " + entity.Name);

            /*if (ImGui.CollapsingHeader("Transform"))
            {
                var numPos = entity.Transform.Position.Numerics();
                if (ImGui.DragFloat3("Position", ref numPos, 0.1f))
                    entity.Transform.Position = numPos.TK();

                var numRot = entity.Transform.Rotation.ToEulerAngles().Numerics();
                if (ImGui.DragFloat3("Rotation", ref numRot, 0.1f))
                    entity.Transform.Rotation = Quaternion.FromEulerAngles(numRot);

                var numScale = entity.Transform.Scale.Numerics();
                if (ImGui.DragFloat3("Scale", ref numScale, 0.1f))
                    entity.Transform.Scale = numScale.TK();
            }*/

            foreach (var comp in entity.GetAllComponents())
            {
                if (comp.GetType().GetCustomAttribute<DisableInspectorAttribute>() != null)
                    continue;

                if (ImGui.CollapsingHeader(comp.Name, ImGuiTreeNodeFlags.DefaultOpen))
                {
                    DrawComponents(comp);
                }
            }
        }

        /*void DrawComponents(Component[] components)
        {
            foreach (var component in components)
                component.OnEditorGui();
        }*/

        void DrawComponents(Component component) // this took too much brainstorming
        {
            var fields = component.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);

            var properties = component.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var methods = component.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance);

            var methods_static = component.GetType().GetMethods(BindingFlags.Public | BindingFlags.Static);

            foreach (var field in fields)
            {
                if (field.GetCustomAttribute<DisableInspectorAttribute>() != null)
                    continue;

                DrawField(component, field);
            }

            foreach (var property in properties)
            {
                if (property.GetCustomAttribute<DisableInspectorAttribute>() != null)
                    continue;

                DrawProperty(component, property);
            }

            foreach (var method in methods)
            {
                if (method.GetCustomAttribute<MethodInspectorAttribute>() == null)
                    continue;

                DrawMethod(component.Entity, component, method);
            }

            foreach (var method in methods_static)
            {
                if (method.GetCustomAttribute<MethodInspectorAttribute>() == null)
                    continue;

                DrawMethod(component.Entity, null, method);
            }
        }

        Dictionary<Entity, Dictionary<MethodInfo, List<object>>> methodParameters = [];

        void DrawMethod(Entity entity, object component, MethodInfo methodInfo) // wrote with no AI first time but eventually fixed it with AI :(
        {
            var parameters = methodInfo.GetParameters();
            var hasParameters = parameters.Length > 0;

            if (hasParameters)
            {
                if (!methodParameters.ContainsKey(entity))
                    methodParameters[entity] = [];

                if (!methodParameters[entity].ContainsKey(methodInfo))
                {
                    methodParameters[entity][methodInfo] = [];

                    foreach (var param in parameters)
                    {
                        var defaultValue = GetDefaultValue(param.ParameterType);
                        methodParameters[entity][methodInfo].Add(defaultValue);
                    }
                }
            }

            if (!hasParameters)
            {
                if (ImGui.Button(methodInfo.Name))
                    methodInfo.Invoke(component, null);

                return;
            }

            if (ImGui.Button(methodInfo.Name))
            {
                methodInfo.Invoke(component, methodParameters[entity][methodInfo].ToArray());
            }

            ImGui.SameLine();

            var isStatic = component == null ? "" : "_static";

            ImGui.PushID($"{methodInfo.Name}_parameter_dropdown{isStatic}");

            if (ImGui.CollapsingHeader($"{methodInfo.Name} Parameters"))
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    ImGui.PushID(methodInfo.Name + "_parameters" + isStatic);

                    var paramInfo = parameters[i];
                    var value = methodParameters[entity][methodInfo][i];

                    methodParameters[entity][methodInfo][i] = DrawParameter(value, paramInfo.Name);

                    ImGui.PopID();
                }
            }

            ImGui.PopID();
        }

        static object GetDefaultValue(Type type)
        {
            if (type == typeof(string))
                return "";

            if (type.IsValueType)
                return Activator.CreateInstance(type);

            return null;
        }

        void DrawField(object component, FieldInfo field)
        {
            var type = field.FieldType;

            if (type == typeof(bool))
            {
                var value = (bool)field.GetValue(component);

                if (ImGui.Checkbox(field.Name, ref value))
                    field.SetValue(component, value);
            }

            else if (type == typeof(string))
            {
                var value = (string)field.GetValue(component);

                if (ImGui.InputText(field.Name, ref value, 1000))
                    field.SetValue(component, value);
            }

            else if (type == typeof(int))
            {
                int value = (int)field.GetValue(component);

                if (ImGui.DragInt(field.Name, ref value))
                    field.SetValue(component, value);
            }

            else if (type == typeof(float))
            {
                var value = (float)field.GetValue(component);

                if (ImGui.DragFloat(field.Name, ref value))
                    field.SetValue(component, value);
            }

            else if (type == typeof(Vector3))
            {
                var value = (Vector3)field.GetValue(component);

                var numValue = value.Numerics();
                if (ImGui.DragFloat3(field.Name, ref numValue))
                    field.SetValue(component, numValue.ChonVec());
            }

            else if (type == typeof(Quaternion))
            {
                var value = (Quaternion)field.GetValue(component);

                var numValue = value.ToEulerAngles().Numerics();
                if (ImGui.DragFloat3(field.Name, ref numValue))
                    field.SetValue(component, Quaternion.FromEulerAngles(numValue));
            }

            else if (type == typeof(Color))
            {
                var value = (Color)field.GetValue(component);

                var numValue = value.Numerics4();
                if (ImGui.ColorEdit4(field.Name, ref numValue))
                    field.SetValue(component, (Color)numValue);
            }

            /*else if (type == typeof(ArrayList))
            {
                var value = (Array)field.GetValue(component);

                DrawArray(value, field.Name);
            }

            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
            {
                var value = field.GetValue(component);

                DrawList(value, field.Name);
            }*/
        }

        void DrawProperty(object component, PropertyInfo property)
        {
            var type = property.PropertyType;

            if (type == typeof(bool))
            {
                var value = (bool)property.GetValue(component);

                if (ImGui.Checkbox(property.Name, ref value))
                    property.SetValue(component, value);
            }

            else if (type == typeof(string))
            {
                var value = (string)property.GetValue(component);

                if (ImGui.InputText(property.Name, ref value, 1000))
                    property.SetValue(component, value);
            }

            else if (type == typeof(int))
            {
                int value = (int)property.GetValue(component);

                if (ImGui.DragInt(property.Name, ref value))
                    property.SetValue(component, value);
            }

            else if (type == typeof(float))
            {
                var value = (float)property.GetValue(component);

                if (ImGui.DragFloat(property.Name, ref value))
                    property.SetValue(component, value);
            }

            else if (type == typeof(Vector3))
            {
                var value = (Vector3)property.GetValue(component);

                var numValue = value.Numerics();
                if (ImGui.DragFloat3(property.Name, ref numValue))
                    property.SetValue(component, numValue.ChonVec());
            }

            else if (type == typeof(Quaternion))
            {
                var value = (Quaternion)property.GetValue(component);

                var numValue = value.ToEulerAngles().Numerics();
                if (ImGui.DragFloat3(property.Name, ref numValue))
                    property.SetValue(component, Quaternion.FromEulerAngles(numValue));
            }

            else if (type == typeof(Color))
            {
                var value = (Color)property.GetValue(component);

                var numValue = value.Numerics4();
                if (ImGui.ColorEdit4(property.Name, ref numValue))
                    property.SetValue(component, (Color)numValue);
            }
        }

        object DrawParameter(object parameter, string name)
        {
            if (parameter == null)
                return null;

            var type = parameter.GetType();

            if (type == typeof(bool))
            {
                bool value = (bool)parameter;
                ImGui.Checkbox(name, ref value);
                return value;
            }

            if (type == typeof(string))
            {
                string value = (string)parameter;
                ImGui.InputText(name, ref value, 256);
                return value;
            }

            if (type == typeof(int))
            {
                int value = (int)parameter;
                ImGui.DragInt(name, ref value);
                return value;
            }

            if (type == typeof(float))
            {
                float value = (float)parameter;
                ImGui.DragFloat(name, ref value);
                return value;
            }

            if (type == typeof(Vector3))
            {
                Vector3 value = (Vector3)parameter;
                var num = value.Numerics();

                if (ImGui.DragFloat3(name, ref num))
                    value = num.ChonVec();

                return value;
            }

            if (type == typeof(Quaternion))
            {
                Quaternion value = (Quaternion)parameter;
                var num = value.ToEulerAngles().Numerics();

                if (ImGui.DragFloat3(name, ref num))
                    value = Quaternion.FromEulerAngles(num);

                return value;
            }

            if (type == typeof(Color))
            {
                Color value = (Color)parameter;
                var num = value.Numerics4();

                if (ImGui.ColorEdit4(name, ref num))
                    value = (Color)num;

                return value;
            }

            ImGui.Text($"Unsupported: {type.Name}");
            return null;
        }

        void DrawArray(object array, string name)
        {
            var elementType = array.GetType().GetElementType();
            var length = ((Array)array).Length;
            if (ImGui.CollapsingHeader(name))
            {
                for (int i = 0; i < length; i++)
                {
                    var element = ((Array)array).GetValue(i);
                    var newValue = DrawParameter(element, $"{name} [{i}]");
                    ((Array)array).SetValue(newValue, i);
                }
            }
        }

        void DrawList(object array, string name)
        {
            var elementType = array.GetType().GetElementType();
            var length = ((Array)array).Length;
            if (ImGui.CollapsingHeader(name))
            {
                for (int i = 0; i < length; i++)
                {
                    var element = ((Array)array).GetValue(i);
                    var newValue = DrawParameter(element, $"{name} [{i}]");
                    ((Array)array).SetValue(newValue, i);
                }
            }
        }
    }

    public static class Inspector
    {
        public static bool NullSelection => Selected == null;

        public static bool IsSelected(Entity entity) => Selected == entity;

        public static void Select(Entity entity) => Selected = entity;

        public static void Deselect() => Selected = null;

        public static Entity? Selected;
    }
}
