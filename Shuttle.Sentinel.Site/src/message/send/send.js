import Component from 'can-component/';
import DefineMap from 'can-define/map/';
import DefineList from 'can-define/list/';
import view from './send.stache!';
import resources from '~/resources';
import Permissions from '~/permissions';
import alerts from '~/alerts';
import localisation from '~/localisation';
import api from '~/api';
import each from 'can-util/js/each/';
import $ from 'jquery';
import Message from '~/models/message';

resources.add('message', { action: 'send', permission: Permissions.Manage.Messages });

export const ViewModel = DefineMap.extend({

});

export default Component.extend({
    tag: 'sentinel-message-send',
    ViewModel,
    view
});