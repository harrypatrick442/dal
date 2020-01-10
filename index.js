const Dal = require('./backend/Dal');
Object.defineProperty(Dal, 'getIntsTable', {
  get: function(){return require('./backend/getIntsTable')}
});
Object.defineProperty(Dal, 'DalDatabases', {
  get: function(){return require('./backend/DalDatabases')}
});
Object.defineProperty(Dal, 'Mssql', {
  get: function(){return require('./backend/Mssql')}
});
Object.defineProperty(Dal, 'Sqlite', {
  get: function(){return require('./backend/Sqlite')}
});
module.exports= Dal;