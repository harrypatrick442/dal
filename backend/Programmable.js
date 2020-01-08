const Strings = require('strings');
const StringHelper = Strings.StringsHelper;
function Programmable(params){
	var definition = initializeDefinition(params.definition);
	if(!params.name)params.name = getNameFromDefinition(definition);
	this.getProgrammableType = function(){
		throwNotImplemented();
	};
	this.getName= function(){
		return params.name;
	};
	this.getDefinition = function(){
		return definition;
	};
	this.getCreateDefinition = function(){
		throwNotImplemented();
	};
	this.getAlterDefinition = function(){
		throwNotImplemented();
	};
};
module.exports = Programmable;
Programmable.fromSqlReader = function(){
	throwNotImplemented();
};
Programmable.fromFile=function(path){
	return new Promise((resolve, reject)=>{
		fs.readFile(path, (sql, err)=>{
			if(err){
				reject(err);
				return;
			}
			if(!sql) {
				reject(new Error('Empty definition'));
				return;
			}
			resolve(new Programmable({definition:sql}));
		});
	});
};
function initializeDefinition(str){
	str = StringsHelper.replaceAll("OLD\\s+Value", str, "New Value");
	str = StringsHelper.replaceAll("Create\\s+View", str, "Alter View");
	str = StringsHelper.replaceAll("Create\\s+Function", str, "Alter Function");
	return StringsHelper.replaceAll("Create\\s+Procedure", str, "Alter Procedure");
}
function getNameFromDefinition(definition){
	return ''
}
function throwNotImplemented(){
	throw new Error('Not implemented');
}