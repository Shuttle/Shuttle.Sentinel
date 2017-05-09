import DefineMap from 'can-define/map/';
import DefineList from 'can-define/list/';
import connect from 'can-connect';
import data from 'can-connect/data/url/';
import constructor from 'can-connect/constructor/';
import map from 'can-connect/can/map/';
import loader from '@loader';
import alerts from '~/alerts';
import localisation from '~/localisation';
import api from '~/api';
import router from '~/router';

const Model = DefineMap.extend(
    'user-role',
    {
        seal: false
    },
    {
        roleName: 'string',
        active: 'boolean',

        toggle: function() {
            var self = this;

            if (this.working) {
                alerts.show({ message: localisation.value('workingMessage'), name: 'working-message' });
                return;
            }

            this.active = !this.active;
            this.working = true;

            api.post('users/setrole',
                {
                    data: {
                        userId: router.data.id,
                        roleName: this.roleName,
                        active: this.active
                    }
                })
                .done(function(response) {
                    if (response.success) {
                        return;
                    }

                    switch (response.failureReason.toLowerCase()) {
                        case 'lastadministrator':
                            {
                                self.active = true;
                                self.working = false;

                                alerts.show({
                                    message: localisation.value('user:exceptions.last-administrator'),
                                    name: 'last-administrator',
                                    type: 'danger'
                                });

                                break;
                            }
                    }
                });

            this.viewModel._working();
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

connect([constructor, data, map], {
    url: loader.serviceBaseURL + 'users/{id}/roles',
    Map: Model,
    List: Model.List,
    name: 'user-role'
});

export default Model;