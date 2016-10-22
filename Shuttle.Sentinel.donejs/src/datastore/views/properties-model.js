import state from 'sentinel/state';
import Model from 'sentinel/model';
import validation from 'sentinel/validation';

export default Model.extend({
    define: {
        name: {
            value: '',
            get: function(value) {
                var result = value;

                if (!value) {
                    result = state.get('datastore');

                    if (result) {
                        result = result.attr('name');
                    }
                }

                return result || value;
            }
        },

        connectionString: {
            value: '',
            get: function(value) {
                var result = value;

                if (!value) {
                    result = state.get('datastore');

                    if (result) {
                        result = result.attr('connectionString');
                    }
                }

                return result || value;
            }
        },

        providerName: {
            value: '',
            get: function(value) {
                var result = value;

                if (!value) {
                    result = state.get('datastore');

                    if (result) {
                        result = result.attr('providerName');
                    }
                }

                return result || value;
            }
        },

        nameConstraint: {
            get: function() {
                return validation.get('name', this.attr('name'), {
                    name: {
                        presence: true
                    }
                });
            }
        },

        connectionStringConstraint: {
            get: function() {
                return validation.get('connectionString', this.attr('connectionString'), {
                    connectionString: {
                        presence: true
                    }
                });
            }
        },

        providerNameConstraint: {
            get: function() {
                return validation.get('providerName', this.attr('providerName'), {
                    providerName: {
                        presence: true
                    }
                });
            }
        }
    },

    hasErrors: function() {
        return this.attr('nameConstraint') || this.attr('connectionStringConstraint') || this.attr('providerNameConstraint');
    }
});
