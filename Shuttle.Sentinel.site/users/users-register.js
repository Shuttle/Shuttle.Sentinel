Sentinel.Components.ViewModels.UserRegister = Sentinel.ViewModel.extend({
	init: function () {
		this.validatePresenceOf('email');
		this.validateFormatOf(['email'], /^(([^<>()[\]\.,;:\s@\"]+(\.[^<>()[\]\.,;:\s@\"]+)*)|(\".+\"))@(([^<>()[\]\.,;:\s@\"]+\.)+[^<>()[\]\.,;:\s@\"]{2,})$/i, {
			message: 'invalid email'
		});
		this.validatePresenceOf('password');
	}
}, {
	register: function() {
		alert('odd!');
	},

	submit: function () {
		alert(this.attr('email'));

		if (this.hasErrors()) {
			alert('errors');

			return;
		}

		alert('submit');
	}
});

Sentinel.Components.UserRegister = can.Component.extend({
	tag: 'sentinel-users-register',
	template: can.view('users/users-register.stache'),
	viewModel: Sentinel.Components.ViewModels.UserRegister
});

Sentinel.resources.addResource('users', { action: 'register' });
