path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.GsmSupplier.Web"
appcmd delete site "Wodeyun.Project.GsmSupplier.Web"

appcmd add apppool /name:"Wodeyun.Project.GsmSupplier.Web"
appcmd set apppool "Wodeyun.Project.GsmSupplier.Web"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Project.GsmSupplier.Web"^
                /bindings:http/*:15004:^
                /physicalPath:"%cd%\Wodeyun.Project.GsmSupplier.Web"
appcmd set site "Wodeyun.Project.GsmSupplier.Web"^
                /[path='/'].applicationPool:"Wodeyun.Project.GsmSupplier.Web"
