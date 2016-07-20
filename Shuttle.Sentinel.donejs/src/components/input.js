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
    }),
    events: {
        'inserted': function(el, ev) {
            if (this.viewModel.attr('focus')) {
                el[0].childNodes[0].focus();
            }
        }
    }
});


