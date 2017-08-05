using NUnit.Framework;
using Shuttle.Sentinel.Module;

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
			Assert.AreEqual("endpoint-name", section.EndpointName);
			Assert.AreEqual("some://endpoint/queue", section.InboxWorkQueueUri);
			Assert.AreEqual(25, section.HeartbeatIntervalSeconds);
		}
	}
}