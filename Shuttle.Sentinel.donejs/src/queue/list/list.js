import Component from 'can/component/';
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
            value: [
                {
                    columnTitle: 'queue:queue-uri', 
                    attributeName: 'queueUri'
                },
                {
                    columnTitle: 'remove', 
                    columnClass: 'col-md-1',
                    columnType: 'remove-button',
                    buttonClick: 'remove(id)'
                }
            ]
        }
    },

    init: function() {
        this.refresh();
    },

    add: function() {
        state.goto('queue/add');
    },

    refresh: function() {
        this.get('queues');
    },

    remove: function(id) {
        this.delete(`queues/${id}`)
            .done(function() {
                alerts.show({ message: localisation.value('itemRemovalRequested', { itemName: localisation.value('queue:queue-uri') }) });
            });
    }
});


export default Component.extend({
    tag: 'sentinel-queue-list',
    viewModel: ViewModel,
    template
});