Sentinel.Components.ViewModels.NavigationMenu = Sentinel.ViewModel.extend({}, {
    init: function() {
        this._super();
    }
});

Sentinel.Components.NavigationMenu = can.Component.extend({
    tag: 'navigation-menu',
    template: can.view('sentinel/navigation-menu.stache'),
    viewModel: Sentinel.Components.ViewModels.NavigationMenu
});

Sentinel.resources.addLocaleNamespace('navigation');
Sentinel.resources.addResource('navigation');