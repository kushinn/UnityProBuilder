using System;
using UnityEditor;

namespace UnityProBuilder
{
    public enum Result
    {
        AllMissionsFailed = -1,
        MissionFailed = 0,
        Success = 1,
    }

    public interface IUnityBuildCommand
    {
        Result Build(IUnityBuildConfig config);
    }

    public sealed class DelegateWrapperCommand : IUnityBuildCommand
    {
        private readonly Func<IUnityBuildConfig, Result> command;

        public DelegateWrapperCommand(Func<IUnityBuildConfig, Result> d)
        {
            command = d;
        }

        public Result Build(IUnityBuildConfig config)
        {
            return command(config);
        }
    }

    public sealed class UnityBuildPlayerCommand : IUnityBuildCommand
    {
        public Result Build(IUnityBuildConfig config)
        {
            var reader = new TypeReader(config);

            var options = reader.WriteToObjectProperties<UnityEditor.BuildPlayerOptions>(dest: new UnityEditor.BuildPlayerOptions());
            options.target = config.Platform;
            options.options = config.Options;

            if (options.target == BuildTarget.iOS)
            {
                UnityEditor.PlayerSettings.iOS.buildNumber += 1;
            }

            var report = BuildPipeline.BuildPlayer(options);
            return Result.Success;
        }
    }

    public sealed class UnityExportGradleProjectCommand : IUnityBuildCommand
    {
        public Result Build(IUnityBuildConfig config)
        {
            var reader = new TypeReader(config);

            var options = reader.WriteToObjectProperties<UnityEditor.BuildPlayerOptions>(dest: new UnityEditor.BuildPlayerOptions());
            options.target = config.Platform;
            options.options = config.Options;
            

            var report = BuildPipeline.BuildPlayer(options);
            return Result.Success;
        }
    }

    public sealed class PlayerSettingCommand : IUnityBuildCommand
    {
        public Result Build(IUnityBuildConfig config)
        {
            var reader = new TypeReader(config);
            reader.WriteToStaticProperties<UnityEditor.PlayerSettings>();
            return Result.Success;
        }
    }

    //[Serializable]
    //public struct InvokeMethod
    //{
    //    public string typeName;
    //    public string methodName;
    //    public string argName;
    //    public bool requestArgs;
    //}
}
