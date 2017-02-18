using System;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace DualNumbers
{
    static class Parser
    {
        static readonly Type[] importTypes = { typeof(Math), typeof(Vector),
            typeof(DualNumber), typeof(DualVector), typeof(DualNumberGrad), typeof(DualVectorGrad) };
        static readonly ScriptOptions options = ScriptOptions.Default.WithImports(importTypes.Select(t => t.FullName))
            .WithReferences(importTypes.Select(t => t.GetTypeInfo().Assembly).Distinct());

        public static T Parse<T>(string code)
        {
            return CSharpScript.EvaluateAsync<T>(code, options).Result;
        }
    }
}
