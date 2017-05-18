path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Bf.Verificate.Web"
appcmd delete site "Wodeyun.Bf.Verificate.Web"

appcmd add apppool /name:"Wodeyun.Bf.Verificate.Web"
appcmd set apppool "Wodeyun.Bf.Verificate.Web"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Bf.Verificate.Web"^
                /bindings:http/*:13004:^
                /physicalPath:"%cd%\Wodeyun.Bf.Verificate.Web"
appcmd set site "Wodeyun.Bf.Verificate.Web"^
                /[path='/'].applicationPool:"Wodeyun.Bf.Verificate.Web"
