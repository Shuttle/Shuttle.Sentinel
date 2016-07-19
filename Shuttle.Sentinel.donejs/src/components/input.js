import can from 'can';
import template from './input.stache!';

export default can.Component.extend({
    tag: 'sentinel-input',
    template,
    viewModel: can.Map.extend({
        define: {
            type: {
                get: function(type) {
                    return type || 'text';
                }
            }
        }
    })
});


