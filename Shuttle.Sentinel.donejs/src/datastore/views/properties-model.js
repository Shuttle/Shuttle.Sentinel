import state from 'sentinel/state';
import Model from 'sentinel/model';
import validation from 'sentinel/validation';

export default Model.extend({
    define: {
        name: {
            value: ''
        },

        connectionString: {
            value: ''
        },

        providerName: {
            value: ''
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
    },

    values: function(map) {
        this.automap(map);
    }
});
