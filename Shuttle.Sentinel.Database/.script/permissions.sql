exec RegisterAvailablePermission 'sentinel://data-stores/manage'
exec RegisterAvailablePermission 'sentinel://messages/manage'
exec RegisterAvailablePermission 'sentinel://queues/manage'
exec RegisterAvailablePermission 'sentinel://schedules/manage'
exec RegisterAvailablePermission 'sentinel://subscriptions/manage'

exec RegisterAvailablePermission 'sentinel://dashboard/view'
exec RegisterAvailablePermission 'sentinel://data-stores/view'
exec RegisterAvailablePermission 'sentinel://queues/view'
exec RegisterAvailablePermission 'sentinel://schedules/view'
exec RegisterAvailablePermission 'sentinel://subscriptions/view'

exec RegisterRole 'Anonymous'
exec RegisterRolePermission 'Anonymous', 'sentinel://dashboard/view'