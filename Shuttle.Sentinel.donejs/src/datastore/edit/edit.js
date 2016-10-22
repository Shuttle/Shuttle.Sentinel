import Component from 'can/component/';
import template from './edit.stache!';
import resources from 'sentinel/resources';
import Permissions from 'sentinel/permissions';
import state from 'sentinel/state';
import PropertiesModel from '../views/properties-model';

resources.add('datastore', { action: 'edit', permission: Permissions.Manage.DataStores});

export const ViewModel = PropertiesModel.extend({
    define: {
        properties: {
            Value: PropertiesModel
        }
    },

    init: function() {
        if (!state.get('datastore')) {
            this.close();
        }
    },

    save: function() {
        if (this.attr('properties').hasErrors()) {
            return false;
        }

        this.post('datastores', {
            name: this.attr('properties.name'),
            connectionString: this.attr('properties.connectionString'),
            providerName: this.attr('properties.providerName')
        });

        this.close();

        return false;
    },

    close: function() {
        state.goto('datastore/list');
    }
});

export default Component.extend({
    tag: 'sentinel-datastore-edit',
    viewModel: ViewModel,
    template
});