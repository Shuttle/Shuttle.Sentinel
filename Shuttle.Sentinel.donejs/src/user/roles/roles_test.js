import QUnit from 'steal-qunit';
import { ViewModel } from './roles';

// ViewModel unit tests
QUnit.module('sentinel/user/roles');

QUnit.test('Has message', function(){
  var vm = new ViewModel();
  QUnit.equal(vm.attr('message'), 'This is the sentinel-user-roles component');
});
