using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace SCANS.Analyzers.Tests
{
    internal class IMockableInterface
    {
    }

    [Unmockable()]
    internal class IUnmockableInterface
    {
    }

    internal class MockableClass
    {
    }

    internal class UnmockableAttribute : Attribute
    {
    }

    [Unmockable()]
    internal class UnmockableClass
    {
    }

    public class UnmockableUsageAnalyzerTests
    {
        private static readonly UnmockableUsageAnalyzer Analyzer = new();

        //[Theory]
        //[InlineData(typeof(UnmockableClass), true)]
        //[InlineData(typeof(IUnmockableInterface), true)]
        //[InlineData(typeof(List<UnmockableClass>), true)]
        //[InlineData(typeof(IMockableInterface), false)]
        //[InlineData(typeof(List<MockableClass>), false)]
        //[InlineData(typeof(MockableClass), false)]
        //public void UnmockableClassFails(Type type, bool expected)
        //{
        //    var result = Analyzer.PossiblyMocked(type);
        //    Assert.Equal(expected, result);
        //}
    }
}