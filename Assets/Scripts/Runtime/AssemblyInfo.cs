using System.Runtime.CompilerServices;
using BubbleWand;

[assembly: InternalsVisibleTo(AssemblyInfo.NAMESPACE_EDITOR)]
[assembly: InternalsVisibleTo(AssemblyInfo.NAMESPACE_TESTS)]

namespace BubbleWand {
    static class AssemblyInfo {
        internal const string NAMESPACE_RUNTIME = "BubbleWand";
        internal const string NAMESPACE_EDITOR = "BubbleWand.Editor";
        internal const string NAMESPACE_TESTS = "BubbleWand.Tests";
    }
}
