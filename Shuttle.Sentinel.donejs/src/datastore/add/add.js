import Component from 'can/component/';
import template from './add.stache!';
import resources from 'sentinel/resources';
import Permissions from 'sentinel/permissions';
import state from 'sentinel/state';
import Model from 'sentinel/model';
import validation from 'sentinel/validation';

resources.add('datastore', { action: 'add', permission: Permissions.Manage.DataStores});

export const ViewModel = Model.extend({
    define: {
        name: {
            value: '',
            get: function(value) {
                var result = value;

                if (!value) {
                    result = state.get('datastore-clone');

                    if (result) {
                        result = result.attr('name');
                    }
                }

                return result || value;
            }
        },

        connectionString: {
            value: '',
            get: function(value) {
                var result = value;

                if (!value) {
                    result = state.get('datastore-clone');

                    if (result) {
                        result = result.attr('connectionString');
                    }
                }

                return result || value;
            }
        },

        providerName: {
            value: '',
            get: function(value) {
                var result = value;

                if (!value) {
                    result = state.get('datastore-clone');

                    if (result) {
                        result = result.attr('providerName');
                    }
                }

                return result || value;
            }
        },

        nameConstraint: {
            get: function() {
                return validation.get('name', this.attr('name'), {
                    name: {
                        presence: true
                    }
                });
            }
        },

        connectionStringConstraint: {
            get: function() {
                return validation.get('connectionString', this.attr('connectionString'), {
                    connectionString: {
                        presence: true
                    }
                });
            }
        },

        providerNameConstraint: {
            get: function() {
                return validation.get('providerName', this.attr('providerName'), {
                    providerName: {
                        presence: true
                    }
                });
            }
        }
    },

    hasErrors: function() {
        return this.attr('nameConstraint') || this.attr('connectionStringConstraint') || this.attr('providerNameConstraint');
    },

    add: function() {
        if (this.hasErrors()) {
            return false;
        }

        this.post('datastores', {
            name: this.attr('name'),
            connectionString: this.attr('connectionString'),
            providerName: this.attr('providerName')
        });

        this.close();

        return false;
    },

    close: function() {
        state.goto('datastore/list');
    }
});

export default Component.extend({
    tag: 'sentinel-datastore-add',
    viewModel: ViewModel,
    template
});