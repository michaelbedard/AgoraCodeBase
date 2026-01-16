using System;

namespace _src.Code.Core.Utility
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class RequiredSpriteAttribute : Attribute
    {
        public string Key { get; }
        public RequiredSpriteAttribute(string key)
        {
            Key = key;
        }
    }
}