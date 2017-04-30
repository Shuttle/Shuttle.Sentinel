import Component from 'can-component';
import DefineList from 'can-define/list/';
import DefineMap from 'can-define/map/';
import view from './text.stache!';

var Error = DefineMap.extend({
    message: 'string',
    related: {
        Type: DefineList
    }
});

export const ViewModel = DefineMap.extend(
    'SentinelText', 
    {
        value: { type: 'string', value: '' },
        inputClass: { type: 'string', value: '' },
        formGroupClass: { type: 'string', value: '' },
        errorAttribute: { type: 'string', value: '' },
        errors: {
            Type: DefineList,
            '#': Error
        },
        validationMessage: {
            get: function() {
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

export default Component.extend({
    tag: 'sentinel-text',
    view,
    ViewModel
});


