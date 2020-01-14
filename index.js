const Dal = require('./backend/Dal');
var classNames =['getIntsTable', 'DalDatabases', 'Mssql', 'Sqlite', 'ShardBuilder'];
classNames.forEach((className)=>{
	Object.defineProperty(Dal, className, {
	  get: function(){return require('./'+className+'/')}
	});
});
module.exports= Dal;