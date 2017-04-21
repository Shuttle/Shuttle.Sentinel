﻿import guard from '~/guard';

var click = {
    on: function(viewModel, ev) {
        guard.againstUndefined(viewModel, 'viewModel');

        var click = viewModel.attr('click');
        var clickHandler;
        var context = viewModel.attr('context') || viewModel;

        if (!click) {
            throw new Error('No method has been assigned to the \'click\' attribute.');
        }

        if (typeof click === 'function') {
            clickHandler = click;
        } else {
            clickHandler = context[click];

            if (!clickHandler) {
                throw new Error('The context does not contain a method with name \'' + click + '\'.');
            }
        }

        clickHandler.call(context, viewModel.attr('argument'));

        if (!!ev) {
            ev.stopPropagation();
        }
    }
};

export default click;