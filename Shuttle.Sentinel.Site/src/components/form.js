import Component from 'can-component';
import template from './form.stache!';
import localisation from '~/localisation';

export default Component.extend({
    tag: 'sentinel-form',
    template,
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


