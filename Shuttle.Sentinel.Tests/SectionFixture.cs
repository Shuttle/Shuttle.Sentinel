using System;
using System.Configuration;
using System.IO;
using Shuttle.Core.Infrastructure;

namespace Shuttle.Sentinel.Tests
{
	public class SectionFixture
	{
		protected T GetSection<T>(string name, string file) where T : ConfigurationSection
		{
			return ConfigurationSectionProvider.OpenFile<T>("shuttle", name,
				Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $@".\Configuration\files\{file}"));
		}
	}
}