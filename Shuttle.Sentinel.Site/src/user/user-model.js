import DefineMap from 'can-define/map/';
import DefineList from 'can-define/list/';
import set from 'can-set';
import superMap from 'can-connect/can/super-map/';
import loader from '@loader';

const Model = DefineMap.extend({
    seal: false
}, {
    id: 'string',
    username: 'string',
    dateRegistered: 'date',
    registeredBy: 'string'
});

const algebra = new set.Algebra(
  set.props.id('id')
);

Model.List = DefineList.extend({
    '#': Model
});

Model.connection = superMap({
    url: loader.serviceBaseURL + 'users',
    Map: Model,
    List: Model.List,
    name: 'user',
    algebra
});

export default Model;