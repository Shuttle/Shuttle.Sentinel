import DefineMap from 'can-define/map/';
import DefineList from 'can-define/list/';
import connect from 'can-connect';
import data from 'can-connect/data/url/';
import constructor from 'can-connect/constructor/';
import store from 'can-connect/constructor/store/';
import map from 'can-connect/can/map/';
import loader from '@loader';
import api from '~/api';
import router from '~/router';
import $ from 'jquery';


Model.List = DefineList.extend({
    '#': Model
});

connect([constructor, data, map, store], {
    url: loader.serviceBaseURL + ,
    Map: Model,
    List: Model.List,
    name: 'user-role',
    ajax: $.ajax
});

export default Model;