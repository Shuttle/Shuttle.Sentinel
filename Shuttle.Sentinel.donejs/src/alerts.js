import state from 'sentinel/application-state';

var alerts = {
    _key: 1,

    addSuccess: function(message) {
        this.add({ message: message, type: 'success' });
    },

    addInfo: function(message) {
        this.add({ message: message, type: 'info' });
    },

    addWarning: function(message) {
        this.add({ message: message, type: 'warning' });
    },

    addDanger: function(message) {
        this.add({ message: message, type: 'danger' });
    },

    add: function(options) {
        this._push(options, 'add');
    },

    showSuccess: function(message) {
        this.show({ message: message, type: 'success' });
    },

    showInfo: function(message) {
        this.show({ message: message, type: 'info' });
    },

    showWarning: function(message) {
        this.show({ message: message, type: 'warning' });
    },

    showDanger: function(message) {
        this.show({ message: message, type: 'danger' });
    },

    show: function(options) {
        if (!options) {
            return;
        }

        this.remove({ type: options.type || 'info' });

        this._push(options, 'show');
    },

    remove: function(options) {
        const o = options || {};
        var key = o.key;
        var type = o.type || 'info';
        var removal = !key ? 'type' : 'key';

        state.attr('alerts', state.attr('alerts').filter(function(item) {
            return (removal === 'type' && (item.mode !== 'show' || item.type !== type)) || (removal === 'key' && item.key !== key);
        }));
    },

    _push: function(options, mode) {
        var key = this._key + 1;
        var self = this;

        if (!options || !options.message) {
            return;
        }

        const map = {
            message: options.message,
            type: options.type || 'info',
            mode: mode,
            key: key,
            destroy: function() {
                self.remove({ key: key });
            }
        };

        state.attr('alerts').push(map);

        this._key = key;
    }
};

export default alerts;