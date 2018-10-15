using UnityEditor;

namespace UnityProBuilder
{
    public interface IUnityBuildConfig
    {
        BuildTarget Platform { get; }
        BuildOptions Options { get; }
        int BuildNumberAdd { get; }
    }
}
