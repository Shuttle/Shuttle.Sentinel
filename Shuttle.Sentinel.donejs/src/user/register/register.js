import Component from 'can/component/';
import Map from 'can/map/';
import 'can/map/define/';
import './register.less!';
import template from './register.stache!';
import resources from 'sentinel/resources';
import Permissions from 'sentinel/permissions';

resources.add('user', { action: 'register', permission: Permissions.Register.User });

export const ViewModel = Map.extend({
    define: {
        message: {
            value: 'This is the sentinel-user-register component'
        },
        email: {
            value: 'test'
        }
    }
});

export default Component.extend({
    tag: 'sentinel-user-register',
    viewModel: ViewModel,
    template
});

