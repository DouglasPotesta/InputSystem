using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using UObject = UnityEngine.Object;
using Potesta;
using System.Linq;

[System.AttributeUsage(System.AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
public class ConsoleComandAttribute : Attribute {

}
