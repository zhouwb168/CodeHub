path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Bf.DataMaintenance.Web"
appcmd delete site "Wodeyun.Bf.DataMaintenance.Web"

appcmd add apppool /name:"Wodeyun.Bf.DataMaintenance.Web"
appcmd set apppool "Wodeyun.Bf.DataMaintenance.Web"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Bf.DataMaintenance.Web"^
                /bindings:http/*:13008:^
                /physicalPath:"%cd%\Wodeyun.Bf.DataMaintenance.Web"
appcmd set site "Wodeyun.Bf.DataMaintenance.Web"^
                /[path='/'].applicationPool:"Wodeyun.Bf.DataMaintenance.Web"
