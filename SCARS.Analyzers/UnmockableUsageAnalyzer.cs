using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using System.Linq;

namespace SCARS.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class UnmockableUsageAnalyzer : DiagnosticAnalyzer
{
    private static readonly DiagnosticDescriptor Rule = new(
        id: DiagnosticId,
        title: "Mocking of Unmockable Type",
        messageFormat: "Type '{0}' is marked as [{1}] {2}", // ← supports 3 args now
        category: "SCARS",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: "This type should not be mocked. Use a fake or stub instead."
    );

    private static ITypeSymbol GetUnmockableType(ITypeSymbol symbol)
    {
        if (symbol is INamedTypeSymbol namedType)
        {
            if (namedType
                .GetAttributes()
                .Any(attr => attr.AttributeClass?.Name == "UnmockableAttribute"))
            {
                return namedType;
            }

            foreach (var arg in namedType.TypeArguments)
            {
                var result = GetUnmockableType(arg);
                if (result != null)
                    return result;
            }
        }

        return null;
    }

    private void AnalyzeMockCreation(SyntaxNodeAnalysisContext context)
    {
        if (context.Node is not ObjectCreationExpressionSyntax creation)
            return;

        var type = context.SemanticModel.GetSymbolInfo(creation.Type).Symbol as INamedTypeSymbol;
        if (type is null || !type.Name.StartsWith("Mock") || !type.IsGenericType)
            return;

        if (!type.ContainingNamespace.ToDisplayString().StartsWith("Moq"))
            return;

        var mockedType = type.TypeArguments.FirstOrDefault();
        if (mockedType == null)
            return;

        var offendingType = GetUnmockableType(mockedType);

        if (offendingType is INamedTypeSymbol namedOffender)
        {
            var attr = namedOffender
                .GetAttributes()
                .FirstOrDefault(a => a.AttributeClass?.Name == "UnmockableAttribute");

            var typeName = namedOffender.ToDisplayString();
            var attributeName = "Unmockable";

            var reason = attr?.ConstructorArguments.FirstOrDefault().Value?.ToString()
                ?? "This service should be replaced by a real or fake implementation.";

            var description = $"Type '{typeName}' is marked with [{attributeName}] and cannot be mocked. {reason}";

            // Create a descriptor with a dynamic description
            var dynamicDescriptor = new DiagnosticDescriptor(
                id: Rule.Id,
                title: Rule.Title,
                messageFormat: Rule.MessageFormat,
                category: Rule.Category,
                defaultSeverity: Rule.DefaultSeverity,
                isEnabledByDefault: Rule.IsEnabledByDefault,
                description: description
            );

            var diagnostic = Diagnostic.Create(dynamicDescriptor, creation.GetLocation(), typeName, attributeName, reason);

            context.ReportDiagnostic(diagnostic);
        }
    }

    public const string DiagnosticId = "SCARS001";
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => [Rule];

    public override void Initialize(AnalysisContext context)
    {
        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);

        context.RegisterSyntaxNodeAction(AnalyzeMockCreation, SyntaxKind.ObjectCreationExpression);
    }
}