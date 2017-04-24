import DefineMap from 'can-define/map/';
import Component from 'can-component';
import view from './form.stache!';
import localisation from '~/localisation';

export default Component.extend({
    tag: 'sentinel-form',
    view,
    viewModel: DefineMap.extend({
        title: {
            get: function(title) {
                return localisation.value(title);
            }
        },
        type: {
            get: function(type) {
                return type || '';
            }
        }
    })
});


