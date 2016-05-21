Sentinel.Services.LocalizationService = Sentinel.Service.extend({
	init: function () {
		can.stache.registerSimpleHelper('i18n', function (key) {
			return i18next.t(key);
		});
	},

	start: function (namespaces, callback) {
		i18next.use(new i18nextXHRBackend({
			loadPath: 'locales/{{lng}}/{{ns}}.json?_' + Sentinel.configuration.localeVersion
		}));

		i18next.init({
			debug: Sentinel.DEBUG,
			lng: 'en',
			fallbackLng: 'en',
			ns: namespaces,
			defaultNS: 'sentinel'
		}, callback);
	},

	value: function (key, options) {
		return i18next.t(key, options);
	}
});