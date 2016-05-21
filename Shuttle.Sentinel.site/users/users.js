Sentinel.Components.ViewModels.Users = Sentinel.ViewModel.extend({
}, {
});

Sentinel.Components.Users = can.Component.extend({
	tag: 'sentinel-users',
	template: can.view('users/users.stache'),
	viewModel: Sentinel.Components.ViewModels.Users
});

Sentinel.resources.addLocaleNamespace('users');
Sentinel.resources.addResource('users');
