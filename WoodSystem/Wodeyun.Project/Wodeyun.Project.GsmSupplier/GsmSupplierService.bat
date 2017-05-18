path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.GsmSupplier.Services"
appcmd delete site "Wodeyun.Project.GsmSupplier.Services"

appcmd add apppool /name:"Wodeyun.Project.GsmSupplier.Services"
appcmd set apppool "Wodeyun.Project.GsmSupplier.Services"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Project.GsmSupplier.Services"^
                /bindings:http/*:14004:^
                /physicalPath:"%cd%\Wodeyun.Project.GsmSupplier.Services"
appcmd set site "Wodeyun.Project.GsmSupplier.Services"^
                /[path='/'].applicationPool:"Wodeyun.Project.GsmSupplier.Services"
