Sentinel.Services.CacheService = Sentinel.Service.extend({
	getItem: function(key) {
		var result = localStorage.getItem(key);

		return (result)
			? JSON.parse(result)
			: {};
	},

	setItem: function(key, data) {
		Sentinel.guard.againstUndefined(key, 'key');
		Sentinel.guard.againstUndefined(data, 'data');

		this._adapter.setItem(key, JSON.stringify(data));
	},

	removeItem: function(key) {
		this._adapter.removeItem(key);
	}
});