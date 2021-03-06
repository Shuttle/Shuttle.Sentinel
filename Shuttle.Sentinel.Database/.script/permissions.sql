﻿exec RegisterPermission 'sentinel://data-stores/manage'
exec RegisterPermission 'sentinel://messages/manage'
exec RegisterPermission 'sentinel://queues/manage'
exec RegisterPermission 'sentinel://schedules/manage'
exec RegisterPermission 'sentinel://subscriptions/manage'

exec RegisterPermission 'sentinel://dashboard/view'
exec RegisterPermission 'sentinel://data-stores/view'
exec RegisterPermission 'sentinel://queues/view'
exec RegisterPermission 'sentinel://schedules/view'
exec RegisterPermission 'sentinel://subscriptions/view'

exec RegisterRole 'Anonymous'
exec RegisterRolePermission 'Anonymous', 'sentinel://dashboard/view'