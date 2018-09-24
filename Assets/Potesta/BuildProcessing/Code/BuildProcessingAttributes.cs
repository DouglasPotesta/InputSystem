using System;
namespace Potesta
{
    [System.AttributeUsage(System.AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class RunOnPreProcessScenesBuildAttribute : Attribute
    {
        public int order = 0;

        public RunOnPreProcessScenesBuildAttribute()
        {
            order = 0;
        }
        /// <summary>
        /// Default order is 0.
        /// </summary>
        /// <param name="_order"></param>
        public RunOnPreProcessScenesBuildAttribute(int _order)
        {
            order = _order;
        }
    }
    [System.AttributeUsage(System.AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class RunOnPreProcessBuildAttribute : Attribute
    {
        public int order = 0;

        public RunOnPreProcessBuildAttribute()
        {
            order = 0;
        }
        /// <summary>
        /// Default order is 0.
        /// </summary>
        /// <param name="_order"></param>
        public RunOnPreProcessBuildAttribute(int _order)
        {
            order = _order;
        }
    }
    [System.AttributeUsage(System.AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class RunOnTargetPlatformChangedAttribute : Attribute
    {
        public int order = 0;

        public RunOnTargetPlatformChangedAttribute()
        {
            order = 0;
        }
        public RunOnTargetPlatformChangedAttribute(int _order)
        {
            order = _order;
        }
    }
}