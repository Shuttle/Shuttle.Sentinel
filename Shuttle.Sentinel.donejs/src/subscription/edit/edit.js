import Component from 'can/component/';
import template from './edit.stache!';
import resources from 'sentinel/resources';
import Permissions from 'sentinel/permissions';
import state from 'sentinel/state';
import Model from 'sentinel/model';
import '../views/properties';
import PropertiesModel from '../views/properties-model';

resources.add('subscription', { action: 'edit', permission: Permissions.Manage.subscriptions});

export const ViewModel = Model.extend({
    define: {
        properties: {
            Value: PropertiesModel
        }
    },

    init: function() {
        let subscription = state.get('subscription');

        if (!subscription) {
            this.close();
        } else {
            this.attr('properties').values(subscription);
            this.attr('id', subscription.attr('id'));
        }
    },

    save: function() {
        if (this.attr('properties').hasErrors()) {
            return false;
        }

        this.put('subscriptions', {
            id: this.attr('id'),
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
    tag: 'sentinel-subscription-edit',
    viewModel: ViewModel,
    template
});