$(function () {
	var localizationService = new Sentinel.Services.LocalizationService();
	var securityService = new Sentinel.Services.SecurityService();
	var services = {
		localizationService: localizationService,
		securityService: securityService
	};

	Sentinel.container.register('_localizationService', localizationService);
	Sentinel.container.register('_securityService', securityService);

	Sentinel.applicationState = new Sentinel.ApplicationState(services);

	securityService.fetchAnonymousPermissions()
		.done(function () {
			can.route(':resource');
			can.route(':resource/:action');

			can.route.map(Sentinel.applicationState.route);

			can.each(Sentinel.applicationStartCallbacks, function (callback) {
				callback(services);
			});

			localizationService.start(Sentinel.resources.localeNamespaces, function (err) {
				if (err) {
					throw new Error(err);
				}

				$('#application-container').html(can.view('#application-template', Sentinel.applicationState));

				can.route.ready();

				window.location.hash = securityService.hasPermission(Sentinel.Permissions.States.UserRequired)
					? '#!users/register'
					: '#!dashboard';
			});
		});
});
