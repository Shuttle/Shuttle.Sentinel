# Shuttle.Sentinel.Module

This module sends monitoring and audit messages to a Sentinel endpoint.

The module will attach the `SentinelObserver` to the relevant pipelines to feed the agent endpoint with relevant metrics.

```xml
<configuration>
	<configSections>
		<section name="sentinel" type="Shuttle.Sentinel.Module.sentinelSection, Shuttle.Sentinel.Module"/>
	</configSections>

  <sentinel
	inboxWorkQueueUri="scheme:[//[user[:password]@]host[:port]][/path][?query]"
	endpointName="{entry assembly full type name}"
	heartbeatIntervalSeconds="15" />
</configuration>
```

| Attribute						| Default 	| Description	| 
| ---							| ---		| ---			| 
| `inboxWorkQueueUri`			| (required)	| The inbox work queue uri to send agent messages to. |
| `endpointName`			| Entry Assembly Full Type Name | The name of the endpoint that you want to report to the agent.  Defaults to the entry assemby's full type name. |
| `heartbeatIntervalSeconds`				| 15		| The number of seconds between sending heartbeat notification messages to the sentinel endpoint. |

The module will register/resolve itself using [Shuttle.Core container bootstrapping](http://shuttle.github.io/shuttle-core/overview-container/#bootstrapping).