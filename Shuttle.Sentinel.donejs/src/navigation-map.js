import Permissions from 'sentinel/permissions';

var map = [
    {
        href: '#!dashboard',
        text: 'navigation:dashboard',
        permission: Permissions.View.Dashboard
    },
    {
        href: '#!messages',
        text: 'navigation:messages',
        permission: Permissions.View.Messages
    },
    {
        href: '#!subscriptions',
        text: 'navigation:subscriptions',
        permission: Permissions.View.Subscriptions
    },
    {
        href: '#!datastores',
        text: 'navigation:data-stores',
        permission: Permissions.View.DataStores
    },
    {
        href: '#!queues',
        text: 'navigation:queues',
        permission: Permissions.View.Queues
    },
    {
        text: 'navigation:system',
        items: [
            {
                href: '#!user/list',
                text: 'user:list.title',
                permission: Permissions.View.Users
            },
            {
                href: '#!role/list',
                text: 'role:list.title',
                permission: Permissions.View.Roles
            }
        ]
    }
];

export default map;