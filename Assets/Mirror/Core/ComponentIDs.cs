using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Mirror
{
    public static class ComponentIDs
    {
        public static readonly List<Type> componentTypes;
        private static readonly Dictionary<Type, int> _types;


        static ComponentIDs()
        {
            // Get all Component Types
            componentTypes = new List<Type>();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes();
                foreach (var type in types)
                {
                    if (type.IsSubclassOf(typeof(Component)) && !type.IsAbstract)
                    {
                        if (typeof(NetworkBehaviour).IsAssignableFrom(type))
                            componentTypes.Add(type);                       
                    }
                }
            }

            componentTypes.Sort((x,y)=>x.FullName.CompareTo(y.FullName));
            _types = new Dictionary<Type, int>(componentTypes.Count);
            


            foreach (var componentType in componentTypes)
            {
                _types.Add(componentType, _types.Count);           
            }
        }

        public static int GetCount()
        {
            return _types.Count;
        }

        public static int Get(Type componentType)
        {
            Assert.IsTrue(componentType.IsSubclassOf(typeof(Component)), "Only get IDs of Component Types!");
            return _types[componentType];
        }

        public static System.Type Get(int id)
        {
            Assert.IsTrue(id < componentTypes.Count && id >= 0,$"ComponentIDs Get  {id} faild!");
            return componentTypes[id];
        }
    }
}

