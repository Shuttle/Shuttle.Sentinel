import can from 'can';
import superMap from 'can-connect/can/super-map/';
import tag from 'can-connect/can/tag/';
import 'can/map/define/define';
import configuration from 'sentinel/configuration';

export const Role = can.Map.extend({
    define: {
        rolename: {
            value: 'eben'
        }
    }
});

Role.List = can.List.extend({
    Map: Role
}, {});

export const roleConnection = superMap({
    url: configuration.controllerUrl('roles'),
    idProp: 'id',
    Map: Role,
    List: Role.List,
    name: 'role'
});

tag('role-model', roleConnection);

export default Role;