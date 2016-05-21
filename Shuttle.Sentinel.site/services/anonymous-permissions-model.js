Sentinel.Models.AnonymousPermissions = Sentinel.Model.extend({
	findAll: 'GET ' + Sentinel.Model.getApiUrl('AnonymousPermissions'),
	parseModels: function(result) {
		return result.data;
	},
	parseModel: function(model) {
		return model;
	}
}, {});