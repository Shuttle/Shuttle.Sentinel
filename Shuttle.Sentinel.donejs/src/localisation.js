﻿import can from 'can';
import i18next from 'i18next';
import backend from 'i18next-xhr-backend';
import configuration from 'sentinel/configuration';

let localisation = {
    _initialised: false,
    _namespaces: ['sentinel', 'navigation'],

    start: function(callback) {
        can.stache.registerSimpleHelper('i18n', function (key) {
            return i18next.t(key);
        });

        i18next.use(new backend({
            loadPath: `locales/{{lng}}/{{ns}}.json?_${configuration.localeVersion}`
        }));

        i18next.init({
            debug: (settings && settings.debug) || false,
            lng: 'en',
            fallbackLng: 'en',
            ns: this._namespaces,
            defaultNS: 'sentinel'
        }, (error) => {
            if (!error) {
                this._initialised = true;
            }

            callback(error);
        });
    },

    addNamespace: function(ns, callback) {
        if (this._initialised) {
            i18next.loadNamespaces(ns, callback);
        } else {
            this._namespaces.push(ns);
        }
    },

    value: function (key, options) {
        return i18next.t(key, options);
    }
};

export default localisation;