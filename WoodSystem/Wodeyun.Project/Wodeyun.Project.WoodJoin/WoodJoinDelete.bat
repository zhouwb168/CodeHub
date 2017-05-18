path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.WoodJoin.Services"
appcmd delete site "Wodeyun.Project.WoodJoin.Services"

appcmd delete apppool "Wodeyun.Project.WoodJoin.Web"
appcmd delete site "Wodeyun.Project.WoodJoin.Web"
