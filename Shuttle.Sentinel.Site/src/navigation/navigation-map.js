import Permissions from '~/permissions';

var map = [
    {
        href: '#!dashboard',
        text: 'navigation:dashboard',
        permission: Permissions.View.Dashboard
    },
    {
        href: '#!endpoint/list',
        text: 'navigation:endpoints',
        permission: Permissions.Manage.Monitoring
    },
    {
        text: 'navigation:message-types.title',
        items: [
            {
                href: '#!messagetypehandled/list',
                text: 'navigation:message-types.handled',
                permission: Permissions.Manage.Monitoring
            },
            {
                href: '#!messagetypedispatched/list',
                text: 'navigation:message-types.dispatched',
                permission: Permissions.Manage.Monitoring
            }
        ]
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
    },
    {
        href: '#!schedule/list',
        text: 'navigation:schedules',
        permission: Permissions.View.Schedules
    }
];

export default map;