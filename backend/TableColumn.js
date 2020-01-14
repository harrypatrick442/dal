module.exports = function(params){
	var name = params.name;
	if(!name)throw new Error('No column name provided');
	var type = params.type;
	if(!type)throw new Error('No column type provied');
	this.getName=function(){
		return name;
	};
	this.getType = function(){
		return type;
	};
	this.getPrecision = function(){
		return params.precision;
	};
	this.getScale = function(){
		return params.scale;
	};
	this.getLength= function(){
		return params.length;
	};
	this.getPrimaryKey=function(){
		return params.primaryKey?true:false;
	};
	this.getNullable = function(){
		return params.null?true:false;
	};
};