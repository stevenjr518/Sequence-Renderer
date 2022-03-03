using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public class RenameAttribute : PropertyAttribute
{
    public string name;

    public RenameAttribute(string name) {
        this.name = name;
    }
}


