import QUnit from 'steal-qunit';
import { ViewModel } from './list';

// ViewModel unit tests
QUnit.module('sentinel/role/list');

QUnit.test('Has message', function(){
  var vm = new ViewModel();
  QUnit.equal(vm.attr('message'), 'This is the sentinel-role-list component');
});
