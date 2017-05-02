import DefineMap from 'can-define/map/';
import DefineList from 'can-define/list/';
import set from 'can-set';
import superMap from 'can-connect/can/super-map/';
import loader from '@loader';
import api from '~/api';
import alerts from '~/alerts';
import localisation from '~/localisation';

const Model = DefineMap.extend(
    'user-roles',
    {
        seal: false
    },
    {
        roleName: 'string',
        active: 'boolean',
        rowClass: {
            get: function() {
                return this.active ? 'text-success success' : 'text-muted';
            }
        },

        toggle: function() {
            var self = this;

            if (this.working) {
                alerts.show({ message: localisation.value('workingMessage'), name: 'working-message' });
                return;
            }

            this.attr('active', !this.active);
            this.working = true;

            api.post('users/setrole',
                {
                    data: {
                        userId: state.attr('route.id'),
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
        }
    });

const algebra = new set.Algebra(
    set.props.id('id')
);

Model.List = DefineList.extend({
    '#': Model
});

Model.connection = superMap({
    url: loader.serviceBaseURL + 'users/{id}/roles',
    Map: Model,
    List: Model.List,
    name: 'user',
    algebra
});

export default Model;