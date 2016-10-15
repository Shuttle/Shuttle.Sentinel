import Component from 'can/component/';
import List from 'can/list/';
import template from './list.stache!';
import resources from 'sentinel/resources';
import Permissions from 'sentinel/permissions';
import state from 'sentinel/state';
import Model from 'sentinel/model';
import alerts from 'sentinel/alerts';
import localisation from 'sentinel/localisation';

resources.add('queue', { action: 'list', permission: Permissions.Manage.Queues });

export const ViewModel = Model.extend({
    define: {
        columns: {
            value: new List()
        }
    },

    init: function() {
        const columns = this.attr('columns');
        this.refresh();

        if (!columns.length) {
            columns.push({
                columnTitle: 'clone',
                columnClass: 'col-md-1',
                columnType: 'button',
                buttonTitle: 'clone',
                buttonClick: 'clone',
                buttonContext: this
            });

            columns.push({
                columnTitle: 'queue:queue-uri',
                attributeName: 'uri'
            });

            columns.push({
                columnTitle: 'remove',
                columnClass: 'col-md-1',
                columnType: 'remove-button',
                buttonContext: this,
                buttonClick: 'remove'
            });
        }
    },

    add: function() {
        state.goto('queue/add');
    },

    refresh: function() {
        this.get('queues');
    },

    remove: function(row) {
        this.post('queues/remove', {
                    uri: row.attr('uri')
                }
            )
            .done(function() {
                alerts.show({ message: localisation.value('itemRemovalRequested', { itemName: localisation.value('queue:queue-uri') }), name: 'item-removal' });
            });
    },

    clone: function(row) {
        state.set('queue-clone', row);

        this.add();
    }
});


export default Component.extend({
    tag: 'sentinel-queue-list',
    viewModel: ViewModel,
    template
});