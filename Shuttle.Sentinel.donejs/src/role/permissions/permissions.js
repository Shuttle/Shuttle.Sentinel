import Component from 'can/component/';
import List from 'can/list/';
import Map from 'can/map/';
import template from './permissions.stache!';
import resources from 'sentinel/resources';
import Permissions from 'sentinel/permissions';
import state from 'sentinel/state';
import Model from 'sentinel/model';
import alerts from 'sentinel/alerts';
import localisation from 'sentinel/localisation';

resources.add('role', { action: 'permissions', permission: Permissions.Manage.RolePermissions });

let PermissionModel = Map.extend({
    define: {
        permission: {
            value: ''
        },

        active: {
            value: false
        },

        rowClass: {
            get: function() {
                return this.attr('active') ? 'text-success' : 'text-muted';
            }
        }
    },

    toggle: function() {
        this.attr('active',!this.attr('active'));
    }
});

export const ViewModel = Model.extend({
    define: {
        columns: {
            value: [
                {
                    columnTitle: 'active',
                    columnClass: 'col-md-1',
                    columnType: 'template',
                    template: '<sentinel-toggle ($click)="toggle()" {value}="active"/>'
                },
                {
                    columnTitle: 'role:permission',
                    attributeName: 'permission'
                }
            ]
        },

        permissions: {
            value: new List()
        }
    },

    init: function() {
        this.refresh();
    },

    add: function() {
    },

    refresh: function() {
        var self = this;

        self.attr('permissions').replace(new List());

        this.get('permissions')
            .done(function(permissions) {
                $.each(permissions, function(index, permission) {
                    self.attr('permissions').push(new PermissionModel({
                        permission: permission,
                        active: false
                    }));
                });
            });

        
        //this.get('permissions/' + state.attr('route.id'));
    },

    remove: function(id) {
    }
});

export default Component.extend({
    tag: 'sentinel-role-permissions',
    viewModel: ViewModel,
    template
});