The ORM Generic group of classes allow Plugins etc to have the same
common ORM Model class/object as that used by the ORM Model class
that (will) inherit the generic set of ORM Model classes.

This is so that an assembly may be shared.

At the very least, it provides a way for Plugins to have their own model interfacting with the database.

See

D:\Viev\02-Marketing\01-Product\01-Products\Saecia-Boston-Server
	Saecia is Viev's Server tool that uses Plugins.

D:\Viev\02-Marketing\01-Product\01-Products\VievSoap
	Boston uses Plugins that use SOAP / XML
	Saecia can use the same Plugins that Boston uses.
	SOAP is independent of the Viev Plugin Framework

D:\Viev\02-Marketing\01-Product\01-Products\Boston\Versions\SVN\Boston\ClassesLANGUAGEORMGeneric
	Boston's Plugins can manipulate FBM Models, because Boston (to be) and the Plugins (to be)
	both use a common Fact-Based Modeling .dll assembly.