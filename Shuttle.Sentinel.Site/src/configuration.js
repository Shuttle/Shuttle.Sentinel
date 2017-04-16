import DefineMap from 'can-define/map/';

var configuration = new DefineMap({
    settings: function() {
        if (settings == undefined) {
            throw new Error('The \'settings\' object has not been defined.');
        }

        return settings;
    },

    controllerUrl: function(controller) {
        return this.settings().api + controller;
    }
});

export default configuration;
