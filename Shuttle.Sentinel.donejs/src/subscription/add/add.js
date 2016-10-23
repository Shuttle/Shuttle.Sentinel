import Component from 'can/component/';
import template from './add.stache!';
import resources from 'sentinel/resources';
import Permissions from 'sentinel/permissions';
import state from 'sentinel/state';
import Model from 'sentinel/model';
import PropertiesModel from '../views/properties-model';

resources.add('subscription', { action: 'add', permission: Permissions.Manage.subscriptions});

export const ViewModel = Model.extend({
    define: {
        properties: {
            Value: PropertiesModel
        }
    },

    add: function() {
        if (this.attr('properties').hasErrors()) {
            return false;
        }

        this.post('subscriptions', {
            name: this.attr('properties.name'),
            connectionString: this.attr('properties.connectionString'),
            providerName: this.attr('properties.providerName')
        });

        this.close();

        return false;
    },

    close: function() {
        state.goto('subscription/list');
    }
});

export default Component.extend({
    tag: 'sentinel-subscription-add',
    viewModel: ViewModel,
    template
});