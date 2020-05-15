Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.ServiceModel
Imports System.ServiceModel.Description
Imports BostonWCFServiceLibrary.DuplexService

Namespace DuplexServiceHost
    Class Program
        Friend Shared Sub Main(args As String())
            Console.Title = "Boston Duplex Service Host"

            ' Step 1 Create a URI to serve as the base address.
            ' Ex. "http://localhost:9001/WCFServices/"
            Dim baseAddress As New Uri("http://localhost:9001/WCFServices/")

            ' Step 2 Create a ServiceHost instance
            Dim selfHost As New ServiceHost(GetType(BostonWCFServiceLibrary.DuplexService), baseAddress)

            Try
                ' Step 3 Add a service endpoint.
                selfHost.AddServiceEndpoint(GetType(BostonWCFServiceLibrary.IDuplexService), New WSDualHttpBinding(WSDualHttpSecurityMode.None), "DuplexService")

                ' Step 4 Enable Metadata Exchange and Add MEX endpoint
                Dim smb As New ServiceMetadataBehavior()
                smb.HttpGetEnabled = True
                selfHost.Description.Behaviors.Add(smb)
                selfHost.AddServiceEndpoint(ServiceMetadataBehavior.MexContractName, MetadataExchangeBindings.CreateMexHttpBinding(), Convert.ToString(baseAddress) & "mex")

                ' Step 5 Start the service.
                selfHost.Open()
                Console.WriteLine("The service is ready.")
                Console.WriteLine("Listening at: {0}", baseAddress)
                Console.WriteLine("Press <ENTER> to terminate service.")
                Console.WriteLine()
                Console.ReadLine()

                ' Close the ServiceHostBase to shutdown the service.
                selfHost.Close()
            Catch ce As CommunicationException
                Console.WriteLine("An exception occurred: {0}", ce.Message)
                selfHost.Abort()
            End Try
        End Sub
    End Class
End Namespace
