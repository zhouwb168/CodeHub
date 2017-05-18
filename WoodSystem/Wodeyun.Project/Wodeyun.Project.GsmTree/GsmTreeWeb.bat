path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.GsmTree.Web"
appcmd delete site "Wodeyun.Project.GsmTree.Web"

appcmd add apppool /name:"Wodeyun.Project.GsmTree.Web"
appcmd set apppool "Wodeyun.Project.GsmTree.Web"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Project.GsmTree.Web"^
                /bindings:http/*:15005:^
                /physicalPath:"%cd%\Wodeyun.Project.GsmTree.Web"
appcmd set site "Wodeyun.Project.GsmTree.Web"^
                /[path='/'].applicationPool:"Wodeyun.Project.GsmTree.Web"
