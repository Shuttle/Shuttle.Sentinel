import Component from 'can-component/';
import DefineMap from 'can-define/map/';
import DefineList from 'can-define/list/';
import view from './list.stache!';
import resources from '~/resources';
import Permissions from '~/permissions';
import router from '~/router';
import api from '~/api';
import alerts from '~/alerts';
import localisation from '~/localisation';

resources.add('user', { action: 'list', permission: Permissions.View.Users });

const Map = DefineMap.extend(
    'user',
    {
        seal: false
    },
    {
        id: 'string',
        username: 'string',
        dateRegistered: 'date',
        registeredBy: 'string'
    });

var users = api({
    endpoint: 'users',
    Map: Map
});

export const ViewModel = DefineMap.extend(
    'user-list',
    {
        columns: {
            Value: DefineList
        },

        refreshTimestamp: {
            type: 'string'
        },

        get usersPromise() {
            const refreshTimestamp = this.refreshTimestamp;
            return users.list();
        },

        init: function() {
            const columns = this.columns;

            if (!columns.length) {
                columns.push({
                    columnTitle: 'user:list.roles',
                    columnClass: 'col-md-1',
                    columnType: 'button',
                    buttonTitle: 'user:list.roles',
                    buttonClick: 'roles',
                    buttonContext: this
                });

                columns.push({
                    columnTitle: 'user:username',
                    attributeName: 'username'
                });

                columns.push({
                    columnTitle: 'user:dateRegistered',
                    attributeName: 'dateRegistered'
                });

                columns.push({
                    columnTitle: 'user:registeredBy',
                    attributeName: 'registeredBy'
                });

                columns.push({
                    columnTitle: 'remove',
                    columnClass: 'col-md-1',
                    columnType: 'remove-button',
                    buttonContext: this,
                    buttonClick: 'remove'
                });
            }
        },

        add: function() {
            router.goto('user/register');
        },

        refresh: function() {
            this.refreshTimestamp = Date.now();
        },

        remove: function(user) {
            user.destroy()
                .then(function() {
                    alerts.show({
                        message: localisation.value('itemRemovalRequested',
                            { itemName: localisation.value('user:title') })
                    });
                });
        },

        roles: function(user) {
            router.goto(`user/${user.id}/roles`);
        }
    }
);

export default Component.extend({
    tag: 'sentinel-user-list',
    ViewModel,
    view
});