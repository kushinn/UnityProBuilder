using System.Collections.Generic;

namespace UnityProBuilder
{
    public abstract class UnityBuildPipeline
    {
        private readonly List<IUnityBuildCommand> prepostCommands = new List<IUnityBuildCommand>();
        private readonly List<IUnityBuildCommand> buildCommands = new List<IUnityBuildCommand>();
        private readonly List<IUnityBuildCommand> postCommands = new List<IUnityBuildCommand>();

        private bool transmitting = false;

        public bool AppendPrepostCommands(IUnityBuildCommand command)
        {
            if (transmitting)
            {
                return false;
            }
            prepostCommands.Add(command);
            return true;
        }

        public bool AppendBuildCommands(IUnityBuildCommand command)
        {
            if (transmitting)
            {
                return false;
            }
            buildCommands.Add(command);
            return true;
        }

        public bool AppendPostCommands(IUnityBuildCommand command)
        {
            if (transmitting)
            {
                return false;
            }
            postCommands.Add(command);
            return true;
        }

        private Result Build(Result result, List<IUnityBuildCommand> cmds, IUnityBuildConfig config)
        {
            if (result != Result.AllMissionsFailed)
            {
                for (int i = 0; i < cmds.Count; i++)
                {
                    if ((result = cmds[i].Build(config)) == Result.AllMissionsFailed)
                    {
                        break;
                    }
                }
            }
            return result;
        }

        public virtual Result BeforeBuild(IUnityBuildConfig config)
        {
            return Result.Success;
        }

        public Result Build(IUnityBuildConfig config)
        {
            if (transmitting)
            {
                return Result.AllMissionsFailed;
            }
            transmitting = true;

            Result result = Build(Result.Success, prepostCommands, config);
            result = Build(result, buildCommands, config);
            result = Build(result, postCommands, config);

            transmitting = false;
            return result;
        }
    }
}
