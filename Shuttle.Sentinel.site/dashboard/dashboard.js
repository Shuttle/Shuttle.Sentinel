Sentinel.Components.ViewModels.Dashboard = Sentinel.ViewModel.extend({
}, {
});

Sentinel.Components.Dashboard = can.Component.extend({
	tag: 'sentinel-dashboard',
	template: can.view('dashboard/dashboard.stache'),
	viewModel: Sentinel.Components.ViewModels.Dashbord
});

Sentinel.resources.addLocaleNamespace('dashboard');
Sentinel.resources.addResource('dashboard');
