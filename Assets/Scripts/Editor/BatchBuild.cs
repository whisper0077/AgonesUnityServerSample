using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class BatchBuild
    {
        [MenuItem("Build Tool/Build Server")]
        public static void BuildServer()
        {
            string[] scenes = new[] { "Assets/Scenes/AgonesEchoSample.unity" };
            string dir = "Builds/Server";

            Directory.CreateDirectory(dir);

            BuildPlayerOptions option = new BuildPlayerOptions
            {
                scenes = scenes,
                locationPathName = dir + "/AgonesEchoSampleServer.x86_64",
                target = BuildTarget.StandaloneLinux64,
                options = BuildOptions.EnableHeadlessMode
            };
            BuildPipeline.BuildPlayer(option);
        }

        [MenuItem("Build Tool/Build Client")]
        public static void BuildClient()
        {
            string[] scenes = new[] { "Assets/Scenes/AgonesEchoSample.unity" };
            string dir = "Builds/Client";

            Directory.CreateDirectory(dir);

            BuildPlayerOptions option = new BuildPlayerOptions
            {
                scenes = scenes,
                locationPathName = dir + "/AgonesEchoSampleClient.exe",
                target = BuildTarget.StandaloneWindows64,
                options = BuildOptions.None
            };
            BuildPipeline.BuildPlayer(option);
        }
    }
}

