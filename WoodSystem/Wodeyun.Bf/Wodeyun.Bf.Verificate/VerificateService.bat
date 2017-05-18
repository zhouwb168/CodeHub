path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Bf.Verificate.Services"
appcmd delete site "Wodeyun.Bf.Verificate.Services"

appcmd add apppool /name:"Wodeyun.Bf.Verificate.Services"
appcmd set apppool "Wodeyun.Bf.Verificate.Services"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Bf.Verificate.Services"^
                /bindings:http/*:12004:^
                /physicalPath:"%cd%\Wodeyun.Bf.Verificate.Services"
appcmd set site "Wodeyun.Bf.Verificate.Services"^
                /[path='/'].applicationPool:"Wodeyun.Bf.Verificate.Services"
