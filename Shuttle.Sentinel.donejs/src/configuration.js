import Map from 'can/map/';

var configuration = new Map({
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
