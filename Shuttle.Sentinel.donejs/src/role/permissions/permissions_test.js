import QUnit from 'steal-qunit';
import { ViewModel } from './permissions';

// ViewModel unit tests
QUnit.module('sentinel/role/permissions');

QUnit.test('Has message', function(){
  var vm = new ViewModel();
  QUnit.equal(vm.attr('message'), 'This is the sentinel-role-permissions component');
});
