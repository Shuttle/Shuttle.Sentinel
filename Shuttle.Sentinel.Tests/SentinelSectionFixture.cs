using NUnit.Framework;
using Shuttle.Sentinel.Configuration;

namespace Shuttle.Sentinel.Tests
{
	[TestFixture]
	public class SentinelSectionFixture : SectionFixture
	{
		[Test]
		[TestCase("Sentinel-Full.config")]
		[TestCase("Sentinel-Full-Grouped.config")]
		public void Should_be_able_to_load_a_full_configuration(string file)
		{
			var section = GetSection<SentinelSection>("sentinel", file);

			Assert.IsNotNull(section);
			Assert.AreEqual("authentication-service-type", section.AuthenticationServiceType);
			Assert.AreEqual("authorization-service-type", section.AuthorizationServiceType);
			Assert.AreEqual("connection-string-name", section.ConnectionStringName);
		}
	}
}