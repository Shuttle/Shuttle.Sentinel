var Permissions = {
    Add: {
        Role: 'sentinel://role/add'
    },
    Manage: {
        Roles: 'sentinel://roles/manage',
        Users: 'sentinel://users/manage'
    },
    View: {
        Dashboard: 'sentinel://dashboard/view',
        Messages: 'sentinel://messages/view',
        Subscriptions: 'sentinel://subscriptions/view',
        Queues: 'sentinel://queues/view',
        DataStores: 'sentinel://data-stores/view',
        Roles: 'sentinel://roles/view',
        Users: 'sentinel://users/view'
    },
    Register: {
        User: 'sentinel://user/register'
    }
};

export default Permissions;