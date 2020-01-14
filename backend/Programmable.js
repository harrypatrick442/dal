const Strings = require('strings');
const StringHelper = Strings.StringsHelper;
const fs = require('fs');
function Programmable(params){
	var definition = params.definition;
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
		return parseDefinition(definition, 'Create');
	};
	this.getAlterDefinition = function(){
		return parseDefinition(definition, 'Alter');
	};
	this.getCreateOrAlterDefinition=function(){
		return parseDefinition(definition, 'Create Or Alter');
	};
};
module.exports = Programmable;
Programmable.fromSqlReader = function(){
	throwNotImplemented();
};
Programmable.fromFile=function(path){
	return new Promise((resolve, reject)=>{
		fs.readFile(path, (err, sql)=>{
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
function parseDefinition(str, toDo){
	str = StringsHelper.replaceAll(str, "OLD\\s+Value", "New Value");
	str = StringsHelper.replaceAll(str, "Create\\s+View", toDo+" View");
	str = StringsHelper.replaceAll(str, "Create\\s+Function", toDo+" Function");
	str =  StringsHelper.replaceAll(str, "Create\\s+Procedure", toDo+" Procedure");
	str = StringsHelper.replaceAll(str, "Alter\\s+View", toDo+" View");
	str = StringsHelper.replaceAll(str, "Alter\\s+Function", toDo+" Function");
	return StringsHelper.replaceAll(str, "Alter\\s+Procedure", toDo+" Procedure");
}
function getNameFromDefinition(definition){
	return ''
}
function throwNotImplemented(){
	throw new Error('Not implemented');
}