path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Bf.Function.Web"
appcmd delete site "Wodeyun.Bf.Function.Web"

appcmd add apppool /name:"Wodeyun.Bf.Function.Web"
appcmd set apppool "Wodeyun.Bf.Function.Web"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Bf.Function.Web"^
                /bindings:http/*:13005:^
                /physicalPath:"%cd%\Wodeyun.Bf.Function.Web"
appcmd set site "Wodeyun.Bf.Function.Web"^
                /[path='/'].applicationPool:"Wodeyun.Bf.Function.Web"
