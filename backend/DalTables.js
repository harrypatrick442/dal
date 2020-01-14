module.exports= new (function(){
	const Dal =require('./Dal');
	this.createTable = function(databaseConfiguration, table){
		var dal = new Dal(databaseConfiguration);
		return dal.raw(table.getCreate());
	};
})();
