module.exports= new (function(){
	const Dal =require('./Dal');
	this.createTable = function(databaseConfiguration, table){
		var dal = new Dal(databaseConfiguration);
		console.log(table.getCreate());
		return new Promise((resolve, rejecct)=>{});
		return dal.raw(table.getCreate());
	};
})();
