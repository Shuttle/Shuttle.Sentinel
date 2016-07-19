import QUnit from 'steal-qunit';
import User from './user';

QUnit.module('models/user');

QUnit.test('getList', function(){
  stop();
  User.getList().then(function(items) {
    QUnit.equal(items.length, 2);
    QUnit.equal(items.attr('0.description'), 'First item');
    start();
  });
});
