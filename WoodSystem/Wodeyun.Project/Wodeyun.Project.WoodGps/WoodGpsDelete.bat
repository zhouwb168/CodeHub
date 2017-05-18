path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.WoodGps.Services"
appcmd delete site "Wodeyun.Project.WoodGps.Services"

appcmd delete apppool "Wodeyun.Project.WoodGps.Web"
appcmd delete site "Wodeyun.Project.WoodGps.Web"
