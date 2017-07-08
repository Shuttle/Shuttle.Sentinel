import DefineMap from 'can-define/map/';
import DefineList from 'can-define/list/';

var Error = DefineMap.extend({
    message: 'string',
    related: {
        Type: DefineList
    }
});

export default DefineMap.extend(
    'input-view-model',
    {
        seal: false
    },
    {
        focus: { type: 'boolean', value: false },
        value: { type: 'string', value: '' },
        inputClass: { type: 'string', value: '' },
        formGroupClass: { type: 'string', value: '' },
        errorAttribute: { type: 'string', value: '' },
        errors: {
            Type: DefineList,
            '#': Error
        },
        validationMessage: {
            get () {
                var self = this;
                var message = '';

                if (this.errors) {
                    this.errors.forEach(function(error) {
                        if (error.related.indexOf(self.errorAttribute) > -1) {
                            message = error.message;
                            return false;
                        }

                        return true;
                    });
                }

                return message;
            }
        }
    }
);
