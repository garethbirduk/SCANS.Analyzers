using Microsoft.CodeAnalysis;
using System.Linq;

namespace SCANS.Analyzers
{
    internal class UnmockableTypeHelper
    {
        public ITypeSymbol GetUnmockableType(ITypeSymbol symbol)
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
    }
}