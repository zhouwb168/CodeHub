path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Bf.DataMaintenance.Services"
appcmd delete site "Wodeyun.Bf.DataMaintenance.Services"

appcmd add apppool /name:"Wodeyun.Bf.DataMaintenance.Services"
appcmd set apppool "Wodeyun.Bf.DataMaintenance.Services"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Bf.DataMaintenance.Services"^
                /bindings:http/*:12008:^
                /physicalPath:"%cd%\Wodeyun.Bf.DataMaintenance.Services"
appcmd set site "Wodeyun.Bf.DataMaintenance.Services"^
                /[path='/'].applicationPool:"Wodeyun.Bf.DataMaintenance.Services"
