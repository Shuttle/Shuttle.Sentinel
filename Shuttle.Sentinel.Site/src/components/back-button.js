import Component from 'can-component';
import template from './back-button.stache!';

export default Component.extend({
    tag: 'sentinel-back-button',
    template,
    viewModel: {
        back: function() {
            window.history.back();
        }
    }
});