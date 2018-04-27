var Permissions = {
    Manage: {
        DataStores: 'sentinel://data-stores/manage',
        Messages: 'sentinel://messages/manage',
        Monitoring: 'sentinel://monitoring/manage',
        Queues: 'sentinel://queues/manage',
        Schedules: 'sentinel://schedules/manage',
        Subscriptions: 'sentinel://subscriptions/manage'
    },
    View: {
        Dashboard: 'sentinel://dashboard/view',
        DataStores: 'sentinel://data-stores/view',
        Queues: 'sentinel://queues/view',
        Schedules: 'sentinel://schedules/view',
        Subscriptions: 'sentinel://subscriptions/view'
    }
};

export default Permissions;