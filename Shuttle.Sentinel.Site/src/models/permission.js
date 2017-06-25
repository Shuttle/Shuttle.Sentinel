import DefineMap from 'can-define/map/';
import DefineList from 'can-define/list/';
import connect from 'can-connect';
import data from 'can-connect/data/url/';
import constructor from 'can-connect/constructor/';
import store from 'can-connect/constructor/store/';
import map from 'can-connect/can/map/';
import loader from '@loader';

const Model = DefineMap.extend(
    'permission',
    {
        seal: false
    },
    {
        permission: 'string'
    }
);

Model.List = DefineList.extend({
    '#': Model
});

connect([constructor, data, map, store], {
    url: loader.serviceBaseURL + 'permissions',
    Map: Model,
    List: Model.List,
    name: 'permission',
    ajax: $.ajax
});

export default Model;