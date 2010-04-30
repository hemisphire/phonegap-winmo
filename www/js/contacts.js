/**
 * This class provides access to the device contacts.
 * @constructor
 */
var Contact = function(){
  this.name;
  this.email;
  this.phone;
}

var Contacts = function()
{
  this.records = [];  
}

Contacts.prototype.getAllContacts = function(successCallback, errorCallback, options) {
    var params = [successCallback];
    PhoneGap.exec("contacts", [params]);
};

Contacts.prototype.foundContact = function(name, npa, email)
{
  var contact = new Contact();
  contact.name = name;
  contact.email = email;
  contact.phone = npa;
  this.records.push(contact);
}

if (typeof navigator.contacts == "undefined") navigator.contacts = new Contacts();