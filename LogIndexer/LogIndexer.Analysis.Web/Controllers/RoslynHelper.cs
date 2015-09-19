using System;
using System.IO;
using System.Linq;
using System.Reflection;
using LogIndexer.Analysis.Domain;
using LogIndexer.Core.Data.Transforms;
using LogIndexer.Core.Domain;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Raven.Client;

static internal class RoslynHelper
{
    public static Type Compile(string query, out object obj)
    {
        var syntaxTree = CSharpSyntaxTree.ParseText(@"
using System;
using System.Collections.Generic;
using System.Linq;
using LogIndexer.Analysis.Domain;
using LogIndexer.Core.Data;
using LogIndexer.Core.Domain;
using Raven.Client;
using Raven.Client.Linq;

namespace LogIndexer.RoslynCompiler.Template {
    public class LinqQuery { public static object Compile(IDocumentSession session) { return {0}; } }
}
"
            .Replace("{0}", query));

        var assemblyName = Path.GetRandomFileName();
        MetadataReference[] references =
        {
            MetadataReference.CreateFromFile(typeof (object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof (Enumerable).Assembly.Location),
            MetadataReference.CreateFromFile(typeof (Record).Assembly.Location),
            MetadataReference.CreateFromFile(typeof (WebLog).Assembly.Location),
            MetadataReference.CreateFromFile(typeof (WebLog_Transformer).Assembly.Location),
            MetadataReference.CreateFromFile(typeof (IDocumentSession).Assembly.Location),
        };

        var compilation = CSharpCompilation.Create(
            assemblyName,
            syntaxTrees: new[] {syntaxTree},
            references: references,
            options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        Assembly assembly;
        using (var ms = new MemoryStream())
        {
            var result = compilation.Emit(ms);

            if (!result.Success)
            {
                var failures = result.Diagnostics.Where(diagnostic =>
                    diagnostic.IsWarningAsError ||
                    diagnostic.Severity == DiagnosticSeverity.Error);

                var errors = failures.Select(diagnostic => new
                {
                    diagnostic.Id,
                    Message = diagnostic.GetMessage()
                }).Select(e => new ArgumentException(e.Message));

                throw new AggregateException(errors);
            }
            else
            {
                ms.Seek(0, SeekOrigin.Begin);
                assembly = Assembly.Load(ms.ToArray());
            }
        }
        var type = assembly.GetType("LogIndexer.RoslynCompiler.Template.LinqQuery");
        obj = Activator.CreateInstance(type);
        return type;
    }
}