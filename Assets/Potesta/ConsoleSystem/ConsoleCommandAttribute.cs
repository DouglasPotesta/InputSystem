using System;
using UObject = UnityEngine.Object;
namespace Potesta.Console
{
    /// <summary>
    /// A simple attribute for marking which methods can be called by the console command system.
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ConsoleComandAttribute : Attribute
    {

    }
}