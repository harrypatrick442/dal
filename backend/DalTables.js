module.exports= new (function(){
	const Dal =require('./Dal');
	this.createTable = function(databaseConfiguration, table){
		var dal = new Dal(databaseConfiguration);
		setTimeout(()=>{console.log(table.getCreate());},0);
		return dal.raw(table.getCreate());
	};
})();
