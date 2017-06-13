import Component from 'can-component/';
import DefineMap from 'can-define/map/';
import view from './add.stache!';
import resources from '~/resources';
import Permissions from '~/permissions';
import router from '~/router';
import validator from 'can-define-validate-validatejs';

resources.add('role', { action: 'add', permission: Permissions.Manage.Roles});

export const ViewModel = DefineMap.extend(
    'role-add',
    {
        name: {
            type: 'string',
            validate: {
                presence: true
            }
        },

        hasErrors: function() {
            return !!this.errors();
        },

        add: function() {
            var self = this;

            if (this.hasErrors()) {
                return false;
            }

            this.working = true;

            this.post('roles',
            {
                name: this.attr('name')
            });

            this.close();

            return false;
        },

        close: function() {
            state.goto('role/list');
        }
    }
);

export default Component.extend({
    tag: 'sentinel-role-add',
    ViewModel,
    view
});