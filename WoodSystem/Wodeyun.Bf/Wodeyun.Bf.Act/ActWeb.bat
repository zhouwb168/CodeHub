path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Bf.Act.Web"
appcmd delete site "Wodeyun.Bf.Act.Web"

appcmd add apppool /name:"Wodeyun.Bf.Act.Web"
appcmd set apppool "Wodeyun.Bf.Act.Web"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Bf.Act.Web"^
                /bindings:http/*:13007:^
                /physicalPath:"%cd%\Wodeyun.Bf.Act.Web"
appcmd set site "Wodeyun.Bf.Act.Web"^
                /[path='/'].applicationPool:"Wodeyun.Bf.Act.Web"
