module.exports= new (function(){
	const Dal =require('./Dal');
	this.createTable = function(databaseConfiguration, table){
		var dal = new Dal(databaseConfiguration);
		console.log(table.getUpdate());
		return dal.raw(table.getUpdate());
	};
})();