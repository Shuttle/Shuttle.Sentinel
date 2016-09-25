import state from 'sentinel/state';

var alerts = {
    _key: 1,

    show: function(options) {
        if (!options || !options.message) {
            return;
        }

        if (options.key || options.name) {
            this.remove(options);
        }

        this._push(options);
    },

    remove: function(options) {
        if (!options || (!options.key && !options.name && !options.type)) {
            return;
        }

        state.attr('alerts', state.attr('alerts').filter(function(item) {
            var keep = true;

            if (options.key) {
                keep = item.key !== options.key;
            } else {
                if (options.name) {
                    keep = item.name !== options.name;
                } else {
                    if (options.type) {
                        keep = (item.type || 'info') !== options.type;
                    }
                }
            }

            return keep;
        }));
    },

    _push: function(options, mode) {
        var key = this._key + 1;
        var self = this;
        var expiryDate = new Date();

        if (!options || !options.message) {
            return;
        }

        var type = options.type || 'info';

        expiryDate.setSeconds(expiryDate.getSeconds() + 10);

        const map = {
            message: options.message,
            type: type,
            mode: mode,
            key: key,
            name: options.name,
            expiryDate: expiryDate,
            destroy: function() {
                self.remove({ key: key });
            }
        };

        state.attr('alerts').push(map);

        this._key = key;
    },

    _removeExpiredAlerts: function() {
        var date = new Date();

        if (state) {
            $.each(state.attr('alerts'), function(index, item) {
                if (item.expiryDate && item.expiryDate < date) {
                    item.destroy();
                }
            });
        }

        setTimeout(alerts._removeExpiredAlerts, 500);
    }
};

alerts._removeExpiredAlerts();

export default alerts;