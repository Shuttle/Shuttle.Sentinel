import Component from 'can/component/';
import template from './add.stache!';
import resources from 'sentinel/resources';
import Permissions from 'sentinel/permissions';
import state from 'sentinel/state';
import Model from 'sentinel/model';

import validation from 'sentinel/validation';

resources.add('role', { action: 'add', permission: Permissions.Add.Role});

export const ViewModel = Model.extend({
    define: {
        name: {
            value: ''
        },

        nameConstraint: {
            get: function() {
                return validation.get('name', this.attr('name'), {
                    name: {
                        presence: true
                    }
                });
            }
        }
    },

    hasErrors: function() {
        return this.attr('nameConstraint');
    },

    add: function() {
        if (this.hasErrors()) {
            return false;
        }

        this.post('roles', {
            name: this.attr('name')
        });

        this.close();

        return false;
    },

    close: function() {
        state.goto('role/list');
    }
});

export default Component.extend({
    tag: 'sentinel-role-add',
    viewModel: ViewModel,
    template
});