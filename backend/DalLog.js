module.exports = new (function(){
	const Log = require('log');
	var log = new Log({printToConsole:true, path:'/dal.log'});
	this.error = log.error;
	this.warn = log.warn;
	this.info = log.info;
})();