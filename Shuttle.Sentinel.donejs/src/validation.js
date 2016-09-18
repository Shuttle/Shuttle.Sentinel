import validatejs from 'validate.js';

var validation = {
    get: function(name, value, constraints, options) {
        var o = {};
        var errors;

        o[name] = value;
        
        errors = validatejs(o, constraints, options);

        return !!errors ? errors[name][0] : '';
    }
};

export default validation;