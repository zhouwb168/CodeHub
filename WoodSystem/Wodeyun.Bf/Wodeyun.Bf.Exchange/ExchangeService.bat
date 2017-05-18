path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Bf.Exchange.Services"
appcmd delete site "Wodeyun.Bf.Exchange.Services"

appcmd add apppool /name:"Wodeyun.Bf.Exchange.Services"
appcmd set apppool "Wodeyun.Bf.Exchange.Services"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Bf.Exchange.Services"^
                /bindings:http/*:11002:^
                /physicalPath:"%cd%\Wodeyun.Bf.Exchange.Services"
appcmd set site "Wodeyun.Bf.Exchange.Services"^
                /[path='/'].applicationPool:"Wodeyun.Bf.Exchange.Services"
