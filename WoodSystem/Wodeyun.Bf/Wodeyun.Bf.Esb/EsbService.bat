path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Bf.Esb.Services"
appcmd delete site "Wodeyun.Bf.Esb.Services"

appcmd add apppool /name:"Wodeyun.Bf.Esb.Services"
appcmd set apppool "Wodeyun.Bf.Esb.Services"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Bf.Esb.Services"^
                /bindings:http/*:11001:^
                /physicalPath:"%cd%\Wodeyun.Bf.Esb.Services"
appcmd set site "Wodeyun.Bf.Esb.Services"^
                /[path='/'].applicationPool:"Wodeyun.Bf.Esb.Services"
