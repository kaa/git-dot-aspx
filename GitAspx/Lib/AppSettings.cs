#region License

// Copyright 2010 Jeremy Skinner (http://www.jeremyskinner.co.uk)
//  
// Licensed under the Apache License, Version 2.0 (the "License"); 
// you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at 
// 
// http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software 
// distributed under the License is distributed on an "AS IS" BASIS, 
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
// See the License for the specific language governing permissions and 
// limitations under the License.
// 
// The latest version of this file can be found at http://github.com/JeremySkinner/git-dot-aspx

#endregion

namespace GitAspx.Lib {
	using System;
	using System.Configuration;
	using System.IO;
	using System.Web.Hosting;

	public class AppSettings {
		public DirectoryInfo RepositoriesDirectory { get; set; }
		public bool UploadPack { get; set; }
		public bool ReceivePack { get; set; }
		public bool RunHooksSilently { get; set; }
		public TimeSpan HookTimeout { get; set; }

		public static AppSettings FromAppConfig() {
			var settings = new AppSettings();

			var path = ConfigurationManager.AppSettings["RepositoriesDirectory"];

			if (string.IsNullOrEmpty(path)) {
				throw new InvalidOperationException("The 'Repositories' AppSetting has not been initialised.");
			}
			if(path.StartsWith("~")) {
				path = HostingEnvironment.MapPath(path);
				if(path==null) {
					throw new ArgumentException("Virtual path '{0}' could not be mapped".With(path));
				}
			}
			if (!Directory.Exists(path)) {
				throw new DirectoryNotFoundException(
					string.Format("Could not find the directory '{0}' which is configured as the directory of repositories.", path));
			}

			settings.RepositoriesDirectory = new DirectoryInfo(path);


			var uploadPackRaw = ConfigurationManager.AppSettings["UploadPack"];
			var receivePackRaw = ConfigurationManager.AppSettings["ReceivePack"];

			bool uploadpack;
			bool receivePack;

			if (!string.IsNullOrEmpty(uploadPackRaw) && bool.TryParse(uploadPackRaw, out uploadpack)) {
				settings.UploadPack = uploadpack;
			}

			if (!string.IsNullOrEmpty(receivePackRaw) && bool.TryParse(receivePackRaw, out receivePack)) {
				settings.ReceivePack = receivePack;
			}

			var runHooksSilentlyRaw = ConfigurationManager.AppSettings["runHooksSilently"];
			var hookTimeoutRaw = ConfigurationManager.AppSettings["hookTimeout"];

			bool runHooksSilently;
			TimeSpan hookTimeout;

			if(!string.IsNullOrEmpty(runHooksSilentlyRaw) && bool.TryParse(runHooksSilentlyRaw, out runHooksSilently)) {
				settings.RunHooksSilently = runHooksSilently;
			}
			if(!string.IsNullOrEmpty(hookTimeoutRaw) && TimeSpan.TryParse(hookTimeoutRaw, out hookTimeout)) {
				settings.HookTimeout = hookTimeout;
			} else {
				settings.HookTimeout = TimeSpan.FromMinutes(10);
			}

			return settings;
		}
	}
}