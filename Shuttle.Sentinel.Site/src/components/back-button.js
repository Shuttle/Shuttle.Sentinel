import Component from 'can-component';
import view from './back-button.stache!';

export default Component.extend({
    tag: 'sentinel-back-button',
    view,
    viewModel: {
        back: function() {
            window.history.back();
        }
    }
});