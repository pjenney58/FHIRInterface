using System;
namespace Support.Model
{
	public static class AppRunningIn
	{
		public static bool Docker => Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_DOCKER_CONTAINER") == "true";

		public static bool Windows => OperatingSystem.IsWindows();
		
	}
}

