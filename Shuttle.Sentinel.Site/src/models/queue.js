import DefineMap from 'can-define/map/';
import DefineList from 'can-define/list/';
import connect from 'can-connect';
import data from 'can-connect/data/url/';
import constructor from 'can-connect/constructor/';
import store from 'can-connect/constructor/store/';
import map from 'can-connect/can/map/';
import loader from '@loader';

const Model = DefineMap.extend(
    'queue',
    {
        seal: false
    },
    {
        uri: {
            type: 'string'
        }
    }
);

Model.List = DefineList.extend({
    '#': Model
});

connect([constructor, data, map, store], {
    url: loader.serviceBaseURL + 'queues/{search}',
    Map: Model,
    List: Model.List,
    name: 'queue',
    ajax: $.ajax
});

export default Model;