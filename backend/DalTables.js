module.exports= new (function(){
	const Dal =require('./Dal');
	this.createTable = function(databaseConfiguration, table){
		var dal = new Dal(databaseConfiguration);
		setTimeout(()=>{console.log(table.getUpdate());},0);
		return new Promise(()=>{});//dal.raw(table.getUpdate());
	};
})();
