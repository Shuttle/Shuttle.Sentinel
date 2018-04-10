import Permissions from '~/permissions';

var map = [
    {
        href: '#!dashboard',
        text: 'navigation:dashboard',
        permission: Permissions.View.Dashboard
    },
    {
        text: 'navigation:messages.title',
        items: [
            {
                href: '#!message/manage',
                text: 'navigation:messages.manage',
                permission: Permissions.Manage.Messages
            },
            {
                href: '#!message/send',
                text: 'navigation:messages.send',
                permission: Permissions.Manage.Messages
            },
            {
                href: '#!messageheader/list',
                text: 'navigation:messages.headers',
                permission: Permissions.Manage.Messages
            }
        ]
    },
    {
        href: '#!subscription/list',
        text: 'navigation:subscriptions',
        permission: Permissions.View.Subscriptions
    },
    {
        href: '#!datastore/list',
        text: 'navigation:data-stores',
        permission: Permissions.View.DataStores
    },
    {
        href: '#!queue/list',
        text: 'navigation:queues',
        permission: Permissions.View.Queues
    }
];

export default map;