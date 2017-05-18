path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Bf.Grant.Services"
appcmd delete site "Wodeyun.Bf.Grant.Services"

appcmd add apppool /name:"Wodeyun.Bf.Grant.Services"
appcmd set apppool "Wodeyun.Bf.Grant.Services"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Bf.Grant.Services"^
                /bindings:http/*:12006:^
                /physicalPath:"%cd%\Wodeyun.Bf.Grant.Services"
appcmd set site "Wodeyun.Bf.Grant.Services"^
                /[path='/'].applicationPool:"Wodeyun.Bf.Grant.Services"
