import can from 'can';
import template from './back-button.stache!';

export default can.Component.extend({
    tag: 'sentinel-back-button',
    template,
    viewModel: {
        back: function() {
            alert('go back!');
        }
    }
});