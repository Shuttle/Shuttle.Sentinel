import fixture from 'can-fixture';

const store = fixture.store([{
  email: 0,
  description: 'First item'
}, {
  email: 1,
  description: 'Second item'
}]);

fixture({
  'GET configuration.getApiUrl(&#39;&#39;)': store.findAll,
  'GET configuration.getApiUrl(&#39;&#39;)/{email}': store.findOne,
  'POST configuration.getApiUrl(&#39;&#39;)': store.create,
  'PUT configuration.getApiUrl(&#39;&#39;)/{email}': store.update,
  'DELETE configuration.getApiUrl(&#39;&#39;)/{email}': store.destroy
});

export default store;
