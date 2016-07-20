import can from 'can';
import superMap from 'can-connect/can/super-map/';
import tag from 'can-connect/can/tag/';
import 'can/map/define/define';
import configuration from 'sentinel/configuration';

export const User = can.Map.extend({
  define: {}
});

User.List = can.List.extend({
  Map: User
}, {});

export const userConnection = superMap({
  url: configuration.getApiUrl('users'),
  idProp: 'email',
  Map: User,
  List: User.List,
  name: 'user'
});

tag('user-model', userConnection);

export default User;