Implements the interface as defined in

		D:\Viev\02-Marketing\01-Product\01-Products\Boston\Versions\SVN\Boston\ClassesFBMInterface

NB This Folder is hidden because it is a different project.

To communicate 'with' the Interface to make it do something, use (for example) from within Boston:

    prApplication.PluginInterface.InterfaceCreateValueType(lrXMLValueType)

To make Boston do something from the Interface, implement like so:

	Public Overrides Sub ModelCreateFactType(arModel As [Interface].XMLModel.Model)

	then the Plugin will call, like so:

	Call Me.zrInterface.ModelCreateValueType(lrModel)

To set the Model that is shared with the Interface:

	prApplication.PluginInterface.SharedModel = prApplication.WorkingModel.SharedModel

NB You 'can' write directly to the SharedModel, but the protocol is to use the methods,
	especially if you want to handle events directly in the plugin.	