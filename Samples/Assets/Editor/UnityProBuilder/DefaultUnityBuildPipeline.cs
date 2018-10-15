using System;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace UnityProBuilder
{
    [Serializable]
    public struct DefaultUnityBuildConfig : IUnityBuildConfig
    {
        public string platform;
        public string options;
        public int buildNumberAdd;

        public BuildPlayerOptions buildPlayerOptions;
        public PlayerSettings playerSettings;

        public BuildTarget Platform
        {
            get { return (BuildTarget)Enum.Parse(typeof(BuildTarget), platform); }
        }

        public BuildOptions Options
        {
            get { return (BuildOptions)Enum.Parse(typeof(BuildOptions), options); ; }
        }

        public int BuildNumberAdd
        {
            get { return buildNumberAdd; }
        }

        public void FillWithProject(BuildTarget target)
        {
            platform = target.ToString();
            options = BuildOptions.None.ToString();

            TypeReader reader = new TypeReader(playerSettings);
            reader.CopyFieldsFromProperties<UnityEditor.PlayerSettings>();
            playerSettings = (PlayerSettings) reader.Value;


        }
    }

    [Serializable]
    public struct PlayerSettings
    {
        public string productName;
        public string companyName;
        public string applicationIdentifier;
        public string bundleVersion;
    }

    [Serializable]
    public struct BuildPlayerOptions
    {
        public string locationPathName;
    }

    //public struct BuildOptions
    //{
    //    public string 
    //}

    [Serializable]
    public struct IconConfig
    {
        public string iconDirectory;
    }

    [Serializable]
    public struct SplashConfig
    {
        //public bool enable;
        //public string splashPath;
        //public string background;
        //public string backgroundPortrait;
        //public PlayerSettings.SplashScreen.DrawMode drawMode;
        //public KeyValuePair<string, float>[] logos;
        //public float overlayOpacity;
        //public bool show;
        //public bool showUnityLogo;
        //public PlayerSettings.SplashScreen.UnityLogoStyle unityLogoStyle;
    }

    [Serializable]
    public struct InvokeMethod
    {
        public string typeName;
        public string methodName;
        public string argName;
        public bool requestArgs;
    }

    public class DefaultUnityBuildPipeline : UnityBuildPipeline
    {
        private Assembly assembly;

        public DefaultUnityBuildPipeline()
        {
            assembly = Assembly.GetAssembly(typeof(DefaultUnityBuildPipeline));
        }

        public override Result BeforeBuild(IUnityBuildConfig config)
        {
            AppendPrepostCommands(new PlayerSettingCommand());
            AppendBuildCommands(new UnityBuildPlayerCommand());
            return Result.Success;
        }

        public static void Launch(DefaultUnityBuildConfig config)
        {
            var pipeline = new DefaultUnityBuildPipeline();
            pipeline.BeforeBuild(config);
            pipeline.Build(config);
        }

        public static void Launch(string path)
        {
            if (File.Exists(path))
            {
                DefaultUnityBuildPipeline.Launch(JsonUtility.FromJson<DefaultUnityBuildConfig>(File.ReadAllText(path)));
            }
            else
            {
                DefaultUnityBuildConfig empty = new DefaultUnityBuildConfig();
                File.WriteAllText(path, JsonUtility.ToJson(empty));
            }
            AssetDatabase.Refresh();
        }
    }
}
