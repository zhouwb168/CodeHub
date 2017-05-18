path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Bf.Link.Services"
appcmd delete site "Wodeyun.Bf.Link.Services"

appcmd add apppool /name:"Wodeyun.Bf.Link.Services"
appcmd set apppool "Wodeyun.Bf.Link.Services"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Bf.Link.Services"^
                /bindings:http/*:12001:^
                /physicalPath:"%cd%\Wodeyun.Bf.Link.Services"
appcmd set site "Wodeyun.Bf.Link.Services"^
                /[path='/'].applicationPool:"Wodeyun.Bf.Link.Services"
