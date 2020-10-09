using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WeirdUnitBE.GameLogic
{
    public static class ClassAnalyzer
    {
        
        public static List<Type> GetAllLeafClasses(Type baseClassType)
        {
            // Get all child classes from base class
            // and store them to childClassTypeArray
            Type[] childClassTypeArray = Assembly.GetAssembly(baseClassType)
                .GetTypes()
                .Where(type => type.IsClass && !type.IsAbstract && type.IsSubclassOf(baseClassType))
                .ToArray<Type>();
            
            // Check each child class if they are at the bottom of class hierarchy (Leaf nodes)
            // If so, add the leaf class to leafClassTypeList
            List<Type> leafClassTypeList = new List<Type>();
            foreach(Type childClassType in childClassTypeArray)
            {
                var subClassTypeArray = Assembly
                    .GetAssembly(childClassType)
                    .GetTypes()
                    .Where(t => t.IsSubclassOf(childClassType))
                    .ToArray<Type>();
                if(subClassTypeArray.Length == 0)
                {
                    leafClassTypeList.Add(childClassType);
                }
            }

            return leafClassTypeList; // return leaf classes of the class hierarchy

            #region EXAMPLE
                /*
                                Tower
                       ___________|____________
                     /                          \  
          AttackingTower                  RegeneratingTower
                 |                                |
              /  |  \                        /    |    \
            /    |    \                   /       |       \
        /        |       \              /         |          \
    Default   Fast     Strong        Default     Fast         Strong
    Attacking Attacking Attacking  Regenerating Regenerating Regenerating
    Tower     Tower     Tower         Tower       Tower         Tower


        Method will return  :   DefaultAttackingTower
                                FastAttackingTower
                                StrongAttackingTower
                                DefaultRegeneratingTower
                                FastRegeneratingTower
                                StrongRegeneratingTower
                */
            #endregion
        }
    }
}