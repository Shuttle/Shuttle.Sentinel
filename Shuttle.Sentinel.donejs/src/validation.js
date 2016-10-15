import validatejs from 'validate.js';
import guard from 'sentinel/guard';

var validation = {
    get: function(name, value, constraints, options) {
        var o = {};
        var errors;

        o[name] = value;
        
        errors = validatejs(o, constraints, options);

        if (!errors) {
            return '';
        }

        if (!errors[name] || !errors[name][0]) {
            throw new Error('Erors for attribute name \'' + name + '\' cannot be found.');
        }

        return errors[name][0];
    },

    map: function(map, constraints, options) {
        guard.againstUndefined(map, 'map');
        guard.againstUndefined(constraints, 'maconstraints');

        if (!map.serialize) {
            throw new Error('The \'map\' instance received does not have a \'serialize\' method.');
        }

        return validatejs(map.serialize(), constraints, options);
    },

    item: function(map, constraints, options) {
        var errors;
        var attribute;
        var serialized;

        guard.againstUndefined(map, 'map');
        guard.againstUndefined(constraints, 'maconstraints');

        if (!map.serialize) {
            throw new Error('The \'map\' instance received does not have a \'serialize\' method.');
        }
        
        serialized = map.serialize();

        errors = validatejs(serialized, constraints, options);

        for (attribute in errors) {
            if (!errors.hasOwnProperty(attribute)) {
                continue;
            }

            return !!errors[attribute].length ? errors[attribute][0] : '';
        }
    }
};

export default validation;