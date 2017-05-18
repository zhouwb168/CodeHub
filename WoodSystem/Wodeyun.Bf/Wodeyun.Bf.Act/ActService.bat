path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Bf.Act.Services"
appcmd delete site "Wodeyun.Bf.Act.Services"

appcmd add apppool /name:"Wodeyun.Bf.Act.Services"
appcmd set apppool "Wodeyun.Bf.Act.Services"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Bf.Act.Services"^
                /bindings:http/*:12007:^
                /physicalPath:"%cd%\Wodeyun.Bf.Act.Services"
appcmd set site "Wodeyun.Bf.Act.Services"^
                /[path='/'].applicationPool:"Wodeyun.Bf.Act.Services"
