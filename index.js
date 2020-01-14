const Dal = require('./backend/Dal');
var classNames =['getIntsTable', 'DalDatabases', 'DalProgrammability', 'Mssql', 'Sqlite', 'ShardBuilder', 'Table','TableColumn','TableColumnTypes'];
classNames.forEach((className)=>{
	Object.defineProperty(Dal, className, {
	  get: function(){return require('./backend/'+className)}
	});
});
module.exports= Dal;