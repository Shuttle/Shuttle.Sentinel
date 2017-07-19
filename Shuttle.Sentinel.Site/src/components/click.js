import guard from '~/guard';

var click = {
    on: function(viewModel, ev) {
        guard.againstUndefined(viewModel, 'viewModel');

        const click = viewModel.click;

        if (!click) {
            return;
        }

        var clickHandler;
        const context = viewModel.context || viewModel;

        if (typeof click === 'function') {
            clickHandler = click;
        } else {
            clickHandler = context[click];

            if (!clickHandler) {
                throw new Error(`The context does not contain a method with name '${click}'.`);
            }
        }

        clickHandler.call(context, viewModel.argument);

        if (!!ev) {
            ev.stopPropagation();
        }

        return false;
    }
};

export default click;