Sentinel.Models.RegisterSession = Sentinel.Model.extend({
	create: 'POST ' + Sentinel.Model.getApiUrl('Sessions')
}, {});