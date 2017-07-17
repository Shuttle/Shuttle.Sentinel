import DefineMap from 'can-define/map/';
import DefineList from 'can-define/list/';
import guard from '~/guard';
import logger from '~/logger';
import route from 'can-route';
import loader from '@loader';

var State = DefineMap.extend({
    __previousKey: 'string',
    route: route,
    debug: { type: 'boolean', value: loader.debug },
    data: { Value: DefineList },

    modal: {
        Value: DefineMap.extend({
            confirmation: {
                Value: DefineMap.extend({
                    primaryClick: 'observable',
                    message: 'string'
                })
            }
        })
    },

    push: function(name, value) {
        guard.againstUndefined(name, 'name');

        this.data.push({name: name, value: value});
    },

    pop: function(name) {
        guard.againstUndefined(name, 'name');
        
        let key = 'data.' + name;
        let previousKey = this.__previousKey;
        let result;

        this.data.forEach(function(item) {
            if (item.name === name) {
                result = item.value;

                return false;
            } else {
                return true;
            }
        });

        if (!result) {
            if (key === previousKey) {
                logger.info(`There is no data item available for key '${key}'.  However, your last access was to this key.  Keep in mind that when you call 'pop' the data item is destroyed.  To re-use it you wil need to place it in a local variable.`);
            }
        }

        this.__previousKey = key;

        return result;
    }
});

export default new State();