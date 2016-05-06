Sentinel.Components.ViewModels.CoreLabel = Sentinel.ViewModel.extend({}, {
    label: undefined,

    init: function() {
        this._super();

        this._registerLocalizedAttribute('label');
    }
});

Sentinel.Components.CoreLabel = can.Component.extend({
    tag: 'sentinel-core-label',
    template: can.view('components/core-label.stache'),
    viewModel: Sentinel.Components.ViewModels.CoreLabel
});


