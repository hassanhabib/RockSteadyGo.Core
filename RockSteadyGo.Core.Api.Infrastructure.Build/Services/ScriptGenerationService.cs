// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using ADotNet.Clients;
using ADotNet.Models.Pipelines.GithubPipelines.DotNets;
using ADotNet.Models.Pipelines.GithubPipelines.DotNets.Tasks;
using ADotNet.Models.Pipelines.GithubPipelines.DotNets.Tasks.SetupDotNetTaskV1s;

namespace RockSteadyGo.Core.Api.Infrastructure.Build.Services
{
    public class ScriptGenerationService
    {
        private readonly ADotNetClient adotNetClient;

        public ScriptGenerationService() =>
            this.adotNetClient = new ADotNetClient();

        public void GenerateBuildScript()
        {
            var githubPipeline = new GithubPipeline
            {
                Name = ".Net",

                OnEvents = new Events
                {
                    Push = new PushEvent
                    {
                        Branches = new string[] { "main" }
                    },

                    PullRequest = new PullRequestEvent
                    {
                        Branches = new string[] { "main" }
                    }
                },

                Jobs = new Jobs
                {
                    Build = new BuildJob
                    {
                        RunsOn = BuildMachines.Windows2019,

                        Steps = new List<GithubTask>
                    {
                        new CheckoutTaskV2
                        {
                            Name = "Check Out"
                        },

                        new SetupDotNetTaskV1
                        {
                            Name = "Setup Dot Net Version",

                            TargetDotNetVersion = new TargetDotNetVersion
                            {
                                DotNetVersion = "7.0.100-preview.4.22252.9",
                                IncludePrerelease = true
                            }
                        },

                        new RestoreTask
                        {
                            Name = "Restore"
                        },

                        new DotNetBuildTask
                        {
                            Name = "Build"
                        },

                        new TestTask
                        {
                            Name = "Test"
                        }
                    }
                    }
                }
            };

            string dotNetYamlRelativeFilePath = "../../../../.github/workflows/dotnet.yml";
            string dotNetYamlFullPath = System.IO.Path.GetFullPath(dotNetYamlRelativeFilePath);
            FileInfo dotNetYamlDefinition = new FileInfo(dotNetYamlFullPath);

            if (!dotNetYamlDefinition.Directory.Exists)
            {
                dotNetYamlDefinition.Directory.Create();
            }

            adotNetClient.SerializeAndWriteToFile(
                adoPipeline: githubPipeline,
                path: dotNetYamlRelativeFilePath);
        }
    }
}
