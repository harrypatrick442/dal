const Dal = require('./backend/Dal');
Object.defineProperty(Dal, 'getIntsTable', {
  get: function(){return require('./backend/getIntsTable')}
});
Object.defineProperty(Dal, 'DalDatabases', {
  get: function(){return require('./backend/DalDatabases')}
});
module.exports= Dal;