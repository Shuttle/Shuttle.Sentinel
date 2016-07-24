import can from 'can';
import template from './button.stache!';

export default can.Component.extend({
	tag: 'sentinel-button',
    template,
    viewModel: can.Map.extend({
        define: {
            type: {
                get: function(type) {
                    return type || 'button';
                }
            },
            classType: {
                get: function(type) {
                    return type || 'btn-primary';
                }
            },
            iconName: {
                value: ''
            },
            disabled: {
                value: false
            }
        }
    })
});