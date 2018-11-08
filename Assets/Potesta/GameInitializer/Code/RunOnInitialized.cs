
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Potesta
{
    /// <summary>
    /// Used to call a paramterless static void method after Game Initialization is completed. 
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class RunOnGameInitializedAttribute : System.Attribute
    {
        public int order = 0;

        public RunOnGameInitializedAttribute()
        {
            order = 0;
        }
        public RunOnGameInitializedAttribute(int _order)
        {
            order = _order;
        }
        public static MethodInfo[] CallAllMethods()
        {
            System.Type type = typeof(RunOnGameInitializedAttribute);
            Assembly assemblies = Assembly.GetCallingAssembly();
            List<MethodInfo> methodInfos = new List<MethodInfo>();
            System.Type[] types = assemblies.GetTypes();
            methodInfos.AddRange(types.SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).Where(m => m.GetParameters().Length == 0 && m.GetCustomAttributes(type, false).Length > 0)));
            methodInfos.OrderBy(item => ((RunOnGameInitializedAttribute)item.GetCustomAttributes(type, true).First()).order);

            for (int ii = 0; ii < methodInfos.Count; ii++)
            {
                ParameterInfo[] paramaters = methodInfos[ii].GetParameters();
                if (paramaters.Length == 0 && methodInfos[ii].IsStatic)
                {
                    methodInfos[ii].Invoke(null, new object[] { });
                }
            }

            return methodInfos.ToArray();
        }
    }
}