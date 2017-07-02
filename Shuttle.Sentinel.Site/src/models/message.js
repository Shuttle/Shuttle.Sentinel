import DefineMap from 'can-define/map/';
import DefineList from 'can-define/list/';
import connect from 'can-connect';
import data from 'can-connect/data/url/';
import constructor from 'can-connect/constructor/';
import store from 'can-connect/constructor/store/';
import map from 'can-connect/can/map/';
import loader from '@loader';

const Model = DefineMap.extend(
    'message',
    {
        seal: false
    },
    {
        checked: {
            type: 'boolean',
            value: false
        },

        selected: {
            type: 'boolean',
            value: false
        },

        toggleCheck: function(ev) {
            this.checked = !this.checked;

            ev.stopPropagation();
        },

        messageSelected: function(message) {
            this.viewModel.messageSelected(message);
        }
    }
);

Model.List = DefineList.extend({
    '#': Model
});

connect([constructor, data, map, store], {
    url: loader.serviceBaseURL + 'messages',
    Map: Model,
    List: Model.List,
    name: 'message',
    ajax: $.ajax
});

export default Model;