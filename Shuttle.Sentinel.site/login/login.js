Sentinel.Components.ViewModels.Login = Sentinel.ViewModel.extend({
	init: function () {
        this.validatePresenceOf('email');
        this.validatePresenceOf('password');
    }
}, {
    email: '',
    password: '',
    isEMailDisabled: false,
    isPasswordDisabled: false,

	init: function() {
		this.resolve('_securityService');
	},

    login: function() {
		this._securityService.login(this.attr('email'), this.attr('password'));
	}
});

Sentinel.Components.Login = can.Component.extend({
	tag: 'sentinel-login',
	template: can.view('login/login.stache'),
	viewModel: Sentinel.Components.ViewModels.Login
});

Sentinel.resources.addResource('login', { titleLocaleKey: 'credentials.login' });
