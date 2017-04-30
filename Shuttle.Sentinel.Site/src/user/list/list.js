import Component from 'can-component/';
import DefineMap from 'can-define/map/';
import DefineList from 'can-define/list/';
import view from './list.stache!';
import resources from '~/resources';
import Permissions from '~/permissions';
import state from '~/state';
import Model from '../user-model';
import alerts from '~/alerts';
import localisation from '~/localisation';
import { ColumnMap } from '~/components/table';
import { ColumnList } from '~/components/table';

resources.add('user', { action: 'list', permission: Permissions.View.Users });

export const ViewModel = DefineMap.extend(
    'UserList',
    {
        get usersPromise() {
            return Model.getList({});
        },

        columns: {
            Value: ColumnList
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
            state.goto('user/register');
        },

        refresh: function() {
            //this.get('users');
        },

        remove: function(id) {
            this.delete(`roles/${id}`)
                .done(function() {
                    alerts.show({
                        message: localisation.value('itemRemovalRequested',
                            { itemName: localisation.value('user:title') })
                    });
                });
        },

        roles: function(id) {
            state.goto('user/' + id + '/roles');
        }
    }
);

export default Component.extend({
    tag: 'sentinel-user-list',
    ViewModel,
    view
});