import DefineMap from 'can-define/map/';
import DefineList from 'can-define/list/';
import superMap from 'can-connect/can/super-map/';
import loader from '@loader';

const Model = DefineMap.extend(
    'user-role',
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
        }
   }
);

Model.List = DefineList.extend({
    '#': Model
});

Model.connection = superMap({
    url: loader.serviceBaseURL + 'users/{id}/roles',
    Map: Model,
    List: Model.List,
    name: 'user-role'
});

export default Model;