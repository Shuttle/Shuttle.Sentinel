import {DefineMap,Component} from 'can';
import view from './navigation.stache!';
import access from 'shuttle-access';
import state from '~/state';
import router from '~/router';

var ViewModel = DefineMap.extend({
    hasSecondary() {
        return !!this.title || this.navbar.controls.length > 0;
    },
    access: {
        get() {
            return access;
        }
    },
    title: {
        get() {
            return state.title;
        }
    },
    navbar: {
        get() {
            return state.navbar;
        }
    },
    resources: {
        get(){
            return state.resources;
        }
    },
    logout() {
        this.access.logout();
        router.goto({resource: 'dashboard'});
    }
});

export default Component.extend({
    tag: 'sentinel-navigation',
    view,
    ViewModel
});