using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WeirdUnitBE.GameLogic
{
    
    
    public class  ClassAnalyzer
    {
        public List<Type> GetAllLeafClasses(Type baseClass)
        {
            List<Type> leafClassList = new List<Type>();

            List<Type> hierarchyClassList = GetAllHierarchyClasses(baseClass);
            foreach(Type type in hierarchyClassList)
            {
                if(!HasChildren(type))
                {
                    leafClassList.Add(type);
                }
            }

            return leafClassList;
        }

        private List<Type> GetAllHierarchyClasses(Type baseClass)
        {
            List<Type> hierarchy = new List<Type>();

            hierarchy.Add(baseClass);
            hierarchy.AddRange(GetAllSubClasses(baseClass));

            return hierarchy;
        }

        private List<Type> GetAllSubClasses(Type baseClass)
        {
            List<Type> subClasses = Assembly
                .GetAssembly(baseClass)
                .GetTypes()
                .Where(type => type.IsClass && !type.IsAbstract && type.IsSubclassOf(baseClass))
                .ToList();
            return subClasses;
        }

        private bool HasChildren(Type type)
        {
            return GetAllSubClasses(type).Any();
        }
    }
}