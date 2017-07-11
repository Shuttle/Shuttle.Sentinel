import DefineMap from 'can-define/map/';
import DefineList from 'can-define/list/';
import connect from 'can-connect';
import data from 'can-connect/data/url/';
import constructor from 'can-connect/constructor/';
import store from 'can-connect/constructor/store/';
import map from 'can-connect/can/map/';
import loader from '@loader';
import superMap from 'can-connect/can/super-map/';
import set from 'can-set';

const Model = DefineMap.extend(
    'message-send',
    {
        seal: false
    },
    {
        id: 'string',
        destinationQueueUri: 'string',
        messageType: 'string',
        message: 'string'
    }
);

const algebra = new set.Algebra(
    set.props.id('id')
);

Model.List = DefineList.extend({
    '#': Model
});

//connect([constructor, data, map, store], {
Model.connection = superMap({
    url: loader.serviceBaseURL + 'messages',
    Map: Model,
    List: Model.List,
    name: 'message-send'
});

export default Model;