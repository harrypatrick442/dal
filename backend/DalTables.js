module.exports= new (function(configuration){
	const Dal =require('./Dal');
	const dal = new Dal(configuration);
	this.createTable = function(table){
		return dal.raw(table.getCreate());
	};
})();
