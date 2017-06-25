import DefineMap from 'can-define/map/';
import DefineList from 'can-define/list/';
import connect from 'can-connect';
import data from 'can-connect/data/url/';
import constructor from 'can-connect/constructor/';
import store from 'can-connect/constructor/store/';
import map from 'can-connect/can/map/';
import loader from '@loader';
import alerts from '~/alerts';
import localisation from '~/localisation';
import api from '~/api';
import router from '~/router';
import $ from 'jquery';

const Model = DefineMap.extend(
    'role-permission',
    {
        seal: false
    },
    {
        permission: 'string',
        active: 'boolean',
        working: 'boolean',

        toggle: function() {
            var self = this;

            if (this.working) {
                alerts.show({ message: localisation.value('workingMessage'), name: 'working-message' });
                return;
            }

            this.active = !this.active;
            this.working = true;

            api.post('roles/setpermission',
                {
                    data: {
                        roleId: router.data.id,
                        permission: this.permission,
                        active: this.active
                    }
                })
                .done(function(response) {
                    if (response.success) {
                        return;
                    }
                });

            this.working = true;
        },

        rowClass: {
            get: function() {
                return this.active ? 'text-success success' : 'text-muted';
            }
        }
   }
);

Model.List = DefineList.extend({
    '#': Model
});

connect([constructor, data, map, store], {
    url: loader.serviceBaseURL + 'roles/{id}/permissions',
    Map: Model,
    List: Model.List,
    name: 'user-role',
    ajax: $.ajax
});

export default Model;