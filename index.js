const Dal = require('./backend/Dal');
var classNames =['getIntsTable', 'DalDatabases', 'DalProgrammables', 'Mssql', 'Sqlite', 'ShardBuilder'];
classNames.forEach((className)=>{
	Object.defineProperty(Dal, className, {
	  get: function(){return require('./backend/'+className)}
	});
});
module.exports= Dal;