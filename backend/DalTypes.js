module.exports= new (function(){
	const Dal =require('./Dal');
	this.createTableTypes = function(databaseConfiguration, table){
		var dal = new Dal(databaseConfiguration);
		return dal.raw(table.getCreateTableType());
	};
})();
