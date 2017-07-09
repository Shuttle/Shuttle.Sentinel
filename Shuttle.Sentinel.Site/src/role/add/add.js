import Component from 'can-component/';
import DefineMap from 'can-define/map/';
import view from './add.stache!';
import resources from '~/resources';
import Permissions from '~/permissions';
import router from '~/router';
import Role from '~/models/role';
import validator from 'can-define-validate-validatejs';

resources.add('role', { action: 'add', permission: Permissions.Manage.Roles});

export const ViewModel = DefineMap.extend(
    'role-add',
    {
        working: 'boolean',
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

            const role = new Role({
                name: this.name
            });

            role.save()
                .then(function() {
                    self.working = false;
                });

            this.close();

            return false;
        },

        close: function() {
            router.goto('role/list');
        }
    }
);

validator(ViewModel);

export default Component.extend({
    tag: 'sentinel-role-add',
    ViewModel,
    view
});