import can from 'can';
import template from './form.stache!';
import localisation from 'sentinel/localisation';

export default can.Component.extend({
    tag: 'sentinel-form',
    template,
    viewModel: can.Map.extend({
        define: {
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
        }
    })
});


