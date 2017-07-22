import Component from 'can-component';
import DefineMap from 'can-define/map/';
import DefineList from 'can-define/list/';
import view from './select-input.stache!';

const Option = DefineMap.extend({
    value: { type: 'string', value: '' },
    label: { type: 'string', value: '' }
});

const Options = DefineList.extend({
    '#': Option
});

export const ViewModel = DefineMap.extend({
    options: { Value: Options },

    change (element) {
        this.value = element.value;
    },

    getSelected (viewModel) {
        return viewModel.value === this.value
            ? 'selected'
            : '';
    },

    elementClass: {
        value: ''
    },

    value: { type: 'string', value: '' }
});

export default Component.extend({
    tag: 'sentinel-select-input',
    ViewModel,
    view
});